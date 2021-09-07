using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElBuenSaborAdmin.Data;
using ElBuenSaborAdmin.Models;
using ElBuenSaborAdmin.Viewmodels;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using static System.Net.WebRequestMethods;

namespace ElBuenSaborAdmin.Controllers
{
    [Authorize]
    public class ArticulosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ArticulosController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Articulos
        public async Task<IActionResult> Index(string rubro, string searchString)
        {
            IQueryable<RubroArticulo> rubroQuery = _context.Articulos.Where(r=>r.Disabled.Equals(false)).Select(a => a.RubroArticulo);

            IQueryable<string> rubroNombreQuery = _context.Articulos.Where(r => r.Disabled.Equals(false)).Select(a => a.RubroArticulo.Denominacion);

            var articulos = _context.Articulos.Where(a => a.Disabled.Equals(false))
                .Include(a => a.RubroArticulo).Where(a => a.Disabled.Equals(false))
                .Include(a => a.PreciosVentaArticulos).Where(a => a.Disabled.Equals(false))
                .Include(a => a.Stocks).Where(a => a.Disabled.Equals(false));

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                articulos = articulos.Where(a => a.Denominacion.Contains(searchString));
            }

            if (!string.IsNullOrWhiteSpace(rubro))
            {
                articulos = articulos.Where(a => a.RubroArticulo.Denominacion.Contains(rubro));
            }

            //var applicationDbContext = _context.Articulos.Where(a => a.Disabled.Equals(false)).Include(a => a.RubroArticulo).Where(a => a.Disabled.Equals(false)).Include(a => a.Stocks).Where(a => a.Disabled.Equals(false));
            foreach (var item in articulos)
            {
                item.StockActual = this.CalcularStockActual(item);
            }

            var indexArticuloVM = new IndexArticuloVM
            {
                Rubros = new SelectList(await rubroQuery.Distinct().ToListAsync()),
                RubrosNombres = new SelectList(await rubroNombreQuery.Distinct().ToListAsync()),
                Articulos = await articulos.OrderBy(a => a.Denominacion).ToListAsync()

            };

            return View(indexArticuloVM);
        }

        // GET: Articulos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.RubroArticulo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // GET: Articulos/Create
        public IActionResult Create()
        {
            ViewData["RubroArticuloID"] = new SelectList(_context.RubrosArticulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion");
            return View();
        }

        // POST: Articulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Articulo articulo)
        {

            if (articulo.ImageFile == null)
            {
                articulo.Imagen = "placeholder-600x400.png";
            }
            else
            {
                articulo.Imagen = await SaveImage(articulo.ImageFile);
            }
            
            if (ModelState.IsValid)
            {
                //esto se asegura que si un articulo es manufacturado, no tenga ni stock, y que si o si este a la venta
                if (articulo.EsManufacturado)
                {
                    articulo.StockMinimo = 0;
                    articulo.ALaVenta = true;
                }

                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RubroArticuloID"] = new SelectList(_context.RubrosArticulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", articulo.RubroArticuloID);
            return View(articulo);
        }

        // GET: Articulos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }
            ViewData["RubroArticuloID"] = new SelectList(_context.RubrosArticulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", articulo.RubroArticuloID);
            return View(articulo);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                if (articulo.ImageFile == null)
                {
                    if (articulo.Imagen == null)
                    {
                        articulo.Imagen = "placeholder-600x400.png";
                    }
                }
                else
                {
                    try
                    {
                        DeleteImage(articulo.Imagen);
                        articulo.Imagen = await SaveImage(articulo.ImageFile);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        ViewData["RubroArticuloID"] = new SelectList(_context.RubrosArticulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", articulo.RubroArticuloID);
                        return View(articulo);
                    }
                }

                try
                {
                    //esto se asegura que si un articulo es manufacturado, no tenga ni stock, y que si o si este a la venta
                    if (articulo.EsManufacturado)
                    {
                        articulo.StockMinimo = 0;
                        articulo.ALaVenta = true;
                    }
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RubroArticuloID"] = new SelectList(_context.RubrosArticulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", articulo.RubroArticuloID);
            return View(articulo);
        }

        // GET: Articulos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.RubroArticulo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {

            var articulo = await _context.Articulos.FindAsync(id);

            if (id != articulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    articulo.Disabled = true;
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(articulo);
        }

        private bool ArticuloExists(long id)
        {
            return _context.Articulos.Any(e => e.Id == id);
        }

        private int CalcularStockActual(Articulo articulo)
        {
            var total = 0;

            foreach(var stock in articulo.Stocks)
            {
                total += stock.CantidadDisponible;
            }            

            return total;
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            try
            {
                string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/images/", imageName); //development
                //var imagePath = Path.Combine("http://elbuensabor.ddns.net:81/images/", imageName);
                //var imagePath = Path.Combine("C:/Proyecto/ebsa/wwwroot/images/", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                return imageName;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/images/", imageName); //development
            //var imagePath = Path.Combine("http://elbuensabor.ddns.net:81/images/", imageName);
            //var imagePath = Path.Combine("C:/Proyecto/ebsa/wwwroot/images/", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
