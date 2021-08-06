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
    public class EgresosArticulosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EgresosArticulosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EgresoArticulos
        public async Task<IActionResult> Index(long? idStock, long? idArt)
        {
            var eaVM = new IndexEgresoArticuloVM
            {
                ArticuloID = idArt,
                StockID = idStock,
                EgresosArticulos = await _context.EgresosArticulos.Where(a => a.Disabled.Equals(false))
                .Include(e => e.Stock).Where(a => a.Disabled.Equals(false))
                .Include(e => e.Stock.Articulo).Where(a => a.Disabled.Equals(false))
                .Include(e => e.DetalleFactura).Where(a => a.Disabled.Equals(false))
                .Include(e => e.DetalleFactura.Factura).Where(a => a.Disabled.Equals(false))
                .Where(s => s.StockID == idStock).ToListAsync()
            };
            //var applicationDbContext = _context.EgresosArticulos.Where(a => a.Disabled.Equals(false)).Include(e => e.DetalleFactura).Where(a => a.Disabled.Equals(false)).Include(e => e.Stock).Where(a => a.Disabled.Equals(false));
            //var applicationDbContext = _context.EgresosArticulos.Where(a => a.Disabled.Equals(false));
            return View(eaVM);
        }

        // GET: EgresoArticulos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egresoArticulo = await _context.EgresosArticulos.Where(a => a.Disabled.Equals(false))
                .Include(e => e.Stock).Where(a => a.Disabled.Equals(false))
                .Include(e => e.Stock.Articulo).Where(a => a.Disabled.Equals(false))
                .Include(e => e.DetalleFactura).Where(a => a.Disabled.Equals(false))
                .Include(e => e.DetalleFactura.Factura).Where(a => a.Disabled.Equals(false))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (egresoArticulo == null)
            {
                return NotFound();
            }

            return View(egresoArticulo);
        }

        // GET: EgresoArticulos/Create
        public IActionResult Create()
        {
            ViewData["DetalleFacturaId"] = new SelectList(_context.DetallesFacturas.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            ViewData["StockID"] = new SelectList(_context.Stocks.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            return View();
        }

        // POST: EgresoArticulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CantidadEgresada,StockID,DetalleFacturaId,Disabled")] EgresoArticulo egresoArticulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(egresoArticulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DetalleFacturaId"] = new SelectList(_context.DetallesFacturas.Where(r => r.Disabled.Equals(false)), "Id", "Id", egresoArticulo.DetalleFacturaId);
            ViewData["StockID"] = new SelectList(_context.Stocks.Where(r => r.Disabled.Equals(false)), "Id", "Id", egresoArticulo.StockID);
            return View(egresoArticulo);
        }

        // GET: EgresoArticulos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egresoArticulo = await _context.EgresosArticulos.Where(a => a.Disabled.Equals(false))
                .Include(e => e.Stock).Where(a => a.Disabled.Equals(false))
                .Include(e => e.Stock.Articulo).Where(a => a.Disabled.Equals(false))
                .Include(e => e.DetalleFactura).Where(a => a.Disabled.Equals(false))
                .Include(e => e.DetalleFactura.Factura).Where(a => a.Disabled.Equals(false)).FirstOrDefaultAsync(m => m.Id == id);
            if (egresoArticulo == null)
            {
                return NotFound();
            }
            ViewData["DetalleFacturaId"] = new SelectList(_context.DetallesFacturas.Where(r => r.Disabled.Equals(false)).Include(r => r.Factura).Where(r => r.Disabled.Equals(false)), "Id", "Id", egresoArticulo.DetalleFacturaId);
            ViewData["StockID"] = new SelectList(_context.Stocks.Where(r => r.Disabled.Equals(false)).Include(r => r.Articulo).Where(r => r.Disabled.Equals(false)), "Id", "Id", egresoArticulo.StockID);
            return View(egresoArticulo);
        }

        // POST: EgresoArticulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CantidadEgresada,StockID,DetalleFacturaId,Disabled")] EgresoArticulo egresoArticulo)
        {
            if (id != egresoArticulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(egresoArticulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EgresoArticuloExists(egresoArticulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = egresoArticulo.Id });
            }
            ViewData["DetalleFacturaId"] = new SelectList(_context.DetallesFacturas.Where(r => r.Disabled.Equals(false)).Include(r => r.Factura).Where(r => r.Disabled.Equals(false)), "Id", "Id", egresoArticulo.DetalleFacturaId);
            ViewData["StockID"] = new SelectList(_context.Stocks.Where(r => r.Disabled.Equals(false)).Include(r => r.Articulo).Where(r => r.Disabled.Equals(false)), "Id", "Id", egresoArticulo.StockID);
            return View(egresoArticulo);
        }

        // GET: EgresoArticulos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egresoArticulo = await _context.EgresosArticulos.Where(a => a.Disabled.Equals(false))
                .Include(e => e.Stock).Where(a => a.Disabled.Equals(false))
                .Include(e => e.Stock.Articulo).Where(a => a.Disabled.Equals(false))
                .Include(e => e.DetalleFactura).Where(a => a.Disabled.Equals(false))
                .Include(e => e.DetalleFactura.Factura).Where(a => a.Disabled.Equals(false))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (egresoArticulo == null)
            {
                return NotFound();
            }

            return View(egresoArticulo);
        }

        // POST: EgresoArticulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var egresoArticulo = await _context.EgresosArticulos.FindAsync(id);

            if (id != egresoArticulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    egresoArticulo.Disabled = true;
                    _context.Update(egresoArticulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EgresoArticuloExists(egresoArticulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = egresoArticulo.Id });
            }
            return View(egresoArticulo);
        }

        private bool EgresoArticuloExists(long id)
        {
            return _context.EgresosArticulos.Any(e => e.Id == id);
        }
    }
}
