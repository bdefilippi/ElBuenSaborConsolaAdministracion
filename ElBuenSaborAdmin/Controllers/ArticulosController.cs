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

namespace ElBuenSaborAdmin.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticulosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Articulos
        public async Task<IActionResult> Index(string rubro, string searchString)
        {
            IQueryable<RubroArticulo> rubroQuery = _context.Articulos.Where(r=>r.Disabled.Equals(false)).Select(a => a.RubroArticulo);

            IQueryable<string> rubroNombreQuery = _context.Articulos.Where(r => r.Disabled.Equals(false)).Select(a => a.RubroArticulo.Denominacion);

            var articulos = _context.Articulos.Where(a => a.Disabled.Equals(false)).Include(a => a.RubroArticulo).Where(a => a.Disabled.Equals(false)).Include(a => a.Stocks).Where(a => a.Disabled.Equals(false));

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
                Articulos = await articulos.ToListAsync()

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
        public async Task<IActionResult> Create([Bind("Id,Denominacion,Imagen,UnidadMedida,StockMinimo,ALaVenta,Disabled,RubroArticuloID")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> Edit(long id, [Bind("Id,Denominacion,Imagen,UnidadMedida,StockMinimo,ALaVenta,Disabled,RubroArticuloID")] Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
    }
}
