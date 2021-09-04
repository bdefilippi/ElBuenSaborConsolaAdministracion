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
using Microsoft.AspNetCore.Authorization;

namespace ElBuenSaborAdmin.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index(string estado, string searchString)
        {
            var pedidos = _context.Pedidos.Where(a => a.Disabled.Equals(false))
                .Include(p => p.Cliente).Where(a => a.Disabled.Equals(false))
                .Include(p => p.Domicilio).Where(a => a.Disabled.Equals(false));

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                pedidos = pedidos.Where(a => a.Cliente.Nombre.Contains(searchString) || a.Cliente.Apellido.Contains(searchString));
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                pedidos = pedidos.Where(a => a.Estado.Equals(int.Parse(estado)));
            }


            var indexPedidoVM = new IndexPedidoVM
            {
                Pedidos = await pedidos.OrderByDescending(p => p.Fecha).ToListAsync()

            };

            return View(indexPedidoVM);

            //var applicationDbContext = _context.Pedidos.Where(a => a.Disabled.Equals(false)).Include(p => p.Cliente).Where(a => a.Disabled.Equals(false)).Include(p => p.Domicilio).Where(a => a.Disabled.Equals(false));
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Domicilio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["ClienteID"] = new SelectList(_context.Clientes.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            ViewData["DomicilioID"] = new SelectList(_context.Domicilios.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,Fecha,Estado,HoraEstimadaFin,TipoEnvio,ClienteID,DomicilioID,Disabled")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes.Where(r => r.Disabled.Equals(false)), "Id", "Id", pedido.ClienteID);
            ViewData["DomicilioID"] = new SelectList(_context.Domicilios.Where(r => r.Disabled.Equals(false)), "Id", "Id", pedido.DomicilioID);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes.Where(r => r.Disabled.Equals(false)), "Id", "Id", pedido.ClienteID);
            ViewData["DomicilioID"] = new SelectList(_context.Domicilios.Where(r => r.Disabled.Equals(false)), "Id", "Id", pedido.DomicilioID);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Numero,Fecha,Estado,HoraEstimadaFin,TipoEnvio,ClienteID,DomicilioID,Disabled")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            ViewData["ClienteID"] = new SelectList(_context.Clientes.Where(r => r.Disabled.Equals(false)), "Id", "Id", pedido.ClienteID);
            ViewData["DomicilioID"] = new SelectList(_context.Domicilios.Where(r => r.Disabled.Equals(false)), "Id", "Id", pedido.DomicilioID);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Domicilio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pedido.Disabled = true;
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            return View(pedido);
        }

        private bool PedidoExists(long id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}
