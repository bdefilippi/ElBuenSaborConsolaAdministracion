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
    public class PrecioVentaArticulosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrecioVentaArticulosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PrecioVentaArticulos
        public async Task<IActionResult> Index(long? id)
        {
            var pvaVM = new IndexPrecioVentaArticuloVM
            {
                PrecioVentaArticulos = await _context.PreciosVentaArticulos.Where(a => a.Disabled.Equals(false)).Include(p => p.Articulo).Where(a => a.Disabled.Equals(false)).Where(a => a.ArticuloID == id).OrderByDescending(a => a.Fecha).ToListAsync(),
                ArticuloID = id
            };

            //var applicationDbContext = _context.PreciosVentaArticulos.Where(a => a.Disabled.Equals(false)).Include(p => p.Articulo).Where(a => a.Disabled.Equals(false));
            //return View(await applicationDbContext.ToListAsync());
            return View(pvaVM);
        }

        // GET: PrecioVentaArticulos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var precioVentaArticulo = await _context.PreciosVentaArticulos
                .Include(p => p.Articulo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (precioVentaArticulo == null)
            {
                return NotFound();
            }

            return View(precioVentaArticulo);
        }

        // GET: PrecioVentaArticulos/Create
        public IActionResult Create(long? id)
        {
            var pvaVM = new CrearPrecioVentaArticuloVM
            {
                ArticuloID = (long)id,
                Fecha = DateTime.Now
                
            };

            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion");
            return View(pvaVM);
        }

        // POST: PrecioVentaArticulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearPrecioVentaArticuloVM crearPrecioVentaArticuloVM)
        {
            if (ModelState.IsValid)
            {
                var precioVentaArticulo = new PrecioVentaArticulo
                {
                    ArticuloID = crearPrecioVentaArticuloVM.ArticuloID,
                    Fecha = crearPrecioVentaArticuloVM.Fecha,
                    PrecioVenta = crearPrecioVentaArticuloVM.PrecioVenta
                };
                _context.Add(precioVentaArticulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = precioVentaArticulo.ArticuloID });
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", crearPrecioVentaArticuloVM.ArticuloID);
            return View(crearPrecioVentaArticuloVM);
        }

        // GET: PrecioVentaArticulos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var precioVentaArticulo = await _context.PreciosVentaArticulos.FindAsync(id);
            if (precioVentaArticulo == null)
            {
                return NotFound();
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", precioVentaArticulo.ArticuloID);
            return View(precioVentaArticulo);
        }

        // POST: PrecioVentaArticulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,PrecioVenta,Fecha,ArticuloID")] PrecioVentaArticulo precioVentaArticulo)
        {
            if (id != precioVentaArticulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(precioVentaArticulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrecioVentaArticuloExists(precioVentaArticulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = precioVentaArticulo.ArticuloID });
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", precioVentaArticulo.ArticuloID);
            return View(precioVentaArticulo);
        }

        // GET: PrecioVentaArticulos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var precioVentaArticulo = await _context.PreciosVentaArticulos
                .Include(p => p.Articulo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (precioVentaArticulo == null)
            {
                return NotFound();
            }

            return View(precioVentaArticulo);
        }

        // POST: PrecioVentaArticulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var precioVentaArticulo = await _context.PreciosVentaArticulos.FindAsync(id);

            if (id != precioVentaArticulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    precioVentaArticulo.Disabled = true;
                    _context.Update(precioVentaArticulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrecioVentaArticuloExists(precioVentaArticulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = precioVentaArticulo.ArticuloID });
            }
            return View(precioVentaArticulo);
        }

        private bool PrecioVentaArticuloExists(long id)
        {
            return _context.PreciosVentaArticulos.Any(e => e.Id == id);
        }
    }
}
