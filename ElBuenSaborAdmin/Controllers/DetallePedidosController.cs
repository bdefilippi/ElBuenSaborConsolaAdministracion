using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElBuenSaborAdmin.Data;
using ElBuenSaborAdmin.Models;

namespace ElBuenSaborAdmin.Controllers
{
    public class DetallePedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetallePedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DetallePedidos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DetallesPedidos.Where(a => a.Disabled.Equals(false)).Include(d => d.Articulo).Where(a => a.Disabled.Equals(false)).Include(d => d.Pedido).Where(a => a.Disabled.Equals(false));
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DetallePedidos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallesPedidos
                .Include(d => d.Articulo)
                .Include(d => d.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        // GET: DetallePedidos/Create
        public IActionResult Create()
        {
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            ViewData["PedidoID"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            return View();
        }

        // POST: DetallePedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cantidad,Disabled,PedidoID,ArticuloID")] DetallePedido detallePedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detallePedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detallePedido.ArticuloID);
            ViewData["PedidoID"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detallePedido.PedidoID);
            return View(detallePedido);
        }

        // GET: DetallePedidos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallesPedidos.FindAsync(id);
            if (detallePedido == null)
            {
                return NotFound();
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detallePedido.ArticuloID);
            ViewData["PedidoID"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detallePedido.PedidoID);
            return View(detallePedido);
        }

        // POST: DetallePedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Cantidad,Disabled,PedidoID,ArticuloID")] DetallePedido detallePedido)
        {
            if (id != detallePedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detallePedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallePedidoExists(detallePedido.Id))
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
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detallePedido.ArticuloID);
            ViewData["PedidoID"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detallePedido.PedidoID);
            return View(detallePedido);
        }

        // GET: DetallePedidos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallesPedidos
                .Include(d => d.Articulo)
                .Include(d => d.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        // POST: DetallePedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var detallePedido = await _context.DetallesPedidos.FindAsync(id);

            if (id != detallePedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    detallePedido.Disabled = true;
                    _context.Update(detallePedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallePedidoExists(detallePedido.Id))
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
            return View(detallePedido);
        }

        private bool DetallePedidoExists(long id)
        {
            return _context.DetallesPedidos.Any(e => e.Id == id);
        }
    }
}
