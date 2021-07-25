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
    public class StocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public async Task<IActionResult> Index(long? id)
        {
            var stockVM = new IndexStockVM
            {
                Stocks = await _context.Stocks.Where(a => a.Disabled.Equals(false)).Include(s => s.Articulo).Where(a => a.Disabled.Equals(false)).Where(s => s.ArticuloID == id).OrderByDescending(s => s.FechaCompra).ToListAsync(),
                Articulo = await _context.Stocks.Where(a => a.Disabled.Equals(false)).Select(s => s.Articulo).Where(a => a.Id == id).FirstOrDefaultAsync()
            };

            if (stockVM.Articulo == null) {
                stockVM.Articulo = new Articulo();
            }

            //var applicationDbContext = _context.Stocks.Where(a => a.Disabled.Equals(false)).Include(s => s.Articulo).Where(a => a.Disabled.Equals(false));
            //return View(await applicationDbContext.ToListAsync());

            return View(stockVM);
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Articulo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create(long? id)
        {
            var stockVM = new CrearStockVM
            {
                ArticuloID = (long)id,
                FechaCompra = DateTime.Now
            };

            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            return View(stockVM);
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearStockVM crearStockVM)
        {
            if (ModelState.IsValid)
            {
                var stock = new Stock
                {
                    ArticuloID = crearStockVM.ArticuloID,
                    CantidadCompradorProveedor = crearStockVM.CantidadCompradorProveedor,
                    CantidadDisponible = crearStockVM.CantidadCompradorProveedor,
                    FechaCompra = crearStockVM.FechaCompra,
                    PrecioCompra = crearStockVM.PrecioCompra
                };
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = crearStockVM.ArticuloID });
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Id", crearStockVM.ArticuloID);
            return View(crearStockVM);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Id", stock.ArticuloID);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CantidadCompradorProveedor,PrecioCompra,FechaCompra,CantidadDisponible,ArticuloID")] Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = stock.ArticuloID });
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Id", stock.ArticuloID);
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Articulo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    stock.Disabled = true;
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = stock.ArticuloID });
            }
            return View(stock);
        }

        private bool StockExists(long id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }
}
