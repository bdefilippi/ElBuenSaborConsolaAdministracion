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
    public class DetalleFacturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetalleFacturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DetalleFacturas
        public async Task<IActionResult> Index(long? id)
        {
            var applicationDbContext = _context.DetallesFacturas.Where(a => a.FacturaID == id).Where(a => a.Disabled.Equals(false))
                .Include(d => d.Factura).Where(a => a.Disabled.Equals(false))
                .Include(d => d.DetallePedido).Where(a => a.Disabled.Equals(false));
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DetalleFacturas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFactura = await _context.DetallesFacturas
                .Include(d => d.DetallePedido)
                .Include(d => d.Factura)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleFactura == null)
            {
                return NotFound();
            }

            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Create
        public IActionResult Create()
        {
            ViewData["DetallePedidoID"] = new SelectList(_context.DetallesPedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            ViewData["FacturaID"] = new SelectList(_context.Facturas.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            return View();
        }

        // POST: DetalleFacturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Disabled,FacturaID,DetallePedidoID")] DetalleFactura detalleFactura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleFactura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DetallePedidoID"] = new SelectList(_context.DetallesPedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detalleFactura.DetallePedidoID);
            ViewData["FacturaID"] = new SelectList(_context.Facturas.Where(r => r.Disabled.Equals(false)), "Id", "Id", detalleFactura.FacturaID);
            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFactura = await _context.DetallesFacturas.FindAsync(id);
            if (detalleFactura == null)
            {
                return NotFound();
            }
            ViewData["DetallePedidoID"] = new SelectList(_context.DetallesPedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detalleFactura.DetallePedidoID);
            ViewData["FacturaID"] = new SelectList(_context.Facturas.Where(r => r.Disabled.Equals(false)), "Id", "Id", detalleFactura.FacturaID);
            return View(detalleFactura);
        }

        // POST: DetalleFacturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Disabled,FacturaID,DetallePedidoID")] DetalleFactura detalleFactura)
        {
            if (id != detalleFactura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleFactura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleFacturaExists(detalleFactura.Id))
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
            ViewData["DetallePedidoID"] = new SelectList(_context.DetallesPedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", detalleFactura.DetallePedidoID);
            ViewData["FacturaID"] = new SelectList(_context.Facturas.Where(r => r.Disabled.Equals(false)), "Id", "Id", detalleFactura.FacturaID);
            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFactura = await _context.DetallesFacturas
                .Include(d => d.DetallePedido)
                .Include(d => d.Factura)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleFactura == null)
            {
                return NotFound();
            }

            return View(detalleFactura);
        }

        // POST: DetalleFacturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var detalleFactura = await _context.DetallesFacturas.FindAsync(id);

            if (id != detalleFactura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    detalleFactura.Disabled = true;
                    _context.Update(detalleFactura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleFacturaExists(detalleFactura.Id))
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
            return View(detalleFactura);
        }

        private bool DetalleFacturaExists(long id)
        {
            return _context.DetallesFacturas.Any(e => e.Id == id);
        }
    }
}
