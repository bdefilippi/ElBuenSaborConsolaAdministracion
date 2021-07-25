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
    public class MercadoPagoDatosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MercadoPagoDatosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MercadoPagoDatos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MercadoPagoDatos.Where(a => a.Disabled.Equals(false)).Include(m => m.Pedido).Where(a => a.Disabled.Equals(false));
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MercadoPagoDatos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mercadoPagoDatos = await _context.MercadoPagoDatos
                .Include(m => m.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mercadoPagoDatos == null)
            {
                return NotFound();
            }

            return View(mercadoPagoDatos);
        }

        // GET: MercadoPagoDatos/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            return View();
        }

        // POST: MercadoPagoDatos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdentificadorPago,FechaCreacion,FechaAprobacion,FormaPago,MetodoPago,NroTarjeta,Estado,PedidoId,Disabled")] MercadoPagoDatos mercadoPagoDatos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mercadoPagoDatos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", mercadoPagoDatos.PedidoId);
            return View(mercadoPagoDatos);
        }

        // GET: MercadoPagoDatos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mercadoPagoDatos = await _context.MercadoPagoDatos.FindAsync(id);
            if (mercadoPagoDatos == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", mercadoPagoDatos.PedidoId);
            return View(mercadoPagoDatos);
        }

        // POST: MercadoPagoDatos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,IdentificadorPago,FechaCreacion,FechaAprobacion,FormaPago,MetodoPago,NroTarjeta,Estado,PedidoId,Disabled")] MercadoPagoDatos mercadoPagoDatos)
        {
            if (id != mercadoPagoDatos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mercadoPagoDatos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MercadoPagoDatosExists(mercadoPagoDatos.Id))
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
            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", mercadoPagoDatos.PedidoId);
            return View(mercadoPagoDatos);
        }

        // GET: MercadoPagoDatos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mercadoPagoDatos = await _context.MercadoPagoDatos
                .Include(m => m.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mercadoPagoDatos == null)
            {
                return NotFound();
            }

            return View(mercadoPagoDatos);
        }

        // POST: MercadoPagoDatos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var mercadoPagoDato = await _context.MercadoPagoDatos.FindAsync(id);

            if (id != mercadoPagoDato.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mercadoPagoDato.Disabled = true;
                    _context.Update(mercadoPagoDato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MercadoPagoDatosExists(mercadoPagoDato.Id))
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
            return View(mercadoPagoDato);
        }

        private bool MercadoPagoDatosExists(long id)
        {
            return _context.MercadoPagoDatos.Any(e => e.Id == id);
        }
    }
}
