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
using ClosedXML.Excel;
using System.IO;

namespace ElBuenSaborAdmin.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Clientes.Where(a => a.Disabled.Equals(false)).Include(c => c.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios.Where(r => r.Disabled.Equals(false)), "Id", "NombreUsuario");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Telefono,UsuarioID,Disabled")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios.Where(r => r.Disabled.Equals(false)), "Id", "NombreUsuario", cliente.UsuarioID);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios.Where(r => r.Disabled.Equals(false)), "Id", "NombreUsuario", cliente.UsuarioID);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Nombre,Apellido,Telefono,UsuarioID,Disabled")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios.Where(r => r.Disabled.Equals(false)), "Id", "NombreUsuario", cliente.UsuarioID);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cliente.Disabled = true;
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        private bool ClienteExists(long id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        public async Task<IActionResult> PedidosCliente()
        {
            IQueryable<Cliente> clientesQuery = _context.Clientes.Where(r => r.Disabled.Equals(false));

            var hoy = DateTime.Now;

            var pedidosPorClienteVM = new PedidosPorClienteVM
            {
                Clientes = new SelectList(await clientesQuery.ToListAsync(), "Id", "NombreCompleto"),
                FechaInicio = new DateTime(2020, 1, 1),
                FechaFinal = new DateTime(hoy.Year, hoy.Month, hoy.Day, hoy.Hour, hoy.Minute, 0)

            };

            return View(pedidosPorClienteVM);
        }

        public async Task<IActionResult> GenerarReporte(long clienteID, DateTime fechaInicial, DateTime fechaFinal)
        {
            //Filtrado por cliente
            var cliente = await _context.Clientes.Where(r => r.Disabled.Equals(false)).Where(c => c.Id == clienteID)
                .Include(c => c.Pedidos).ThenInclude(p => p.Domicilio).Where(r => r.Disabled.Equals(false))
                .Include(c => c.Pedidos).ThenInclude(p => p.DetallesPedido).Where(r => r.Disabled.Equals(false))
                .FirstOrDefaultAsync();
            //Pedidos entre fecha inicial y final
            var pedidos = cliente.Pedidos.Where(p => p.Fecha >= fechaInicial && p.Fecha <= fechaFinal);

            //Create an instance of ExcelEngine
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Pedidos");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Número";
                worksheet.Cell(currentRow, 2).Value = "Fecha";
                worksheet.Cell(currentRow, 3).Value = "Estado";
                worksheet.Cell(currentRow, 4).Value = "Tipo";
                worksheet.Cell(currentRow, 5).Value = "Domicilio";
                worksheet.Cell(currentRow, 6).Value = "Total";
                foreach (var pedido in pedidos)
                {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = pedido.Numero;
                        worksheet.Cell(currentRow, 2).Value = pedido.Fecha;
                        worksheet.Cell(currentRow, 3).Value = pedido.GetEstadoPedido;
                        worksheet.Cell(currentRow, 4).Value = pedido.GetTipoEnvio;
                        worksheet.Cell(currentRow, 5).Value = pedido.Domicilio.GetDomicilioCompleto;
                        worksheet.Cell(currentRow, 6).Value = "$ " + pedido.GetTotal;
                    
                }

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).AdjustToContents();
                worksheet.Column(3).AdjustToContents();
                worksheet.Column(4).AdjustToContents();
                worksheet.Column(5).AdjustToContents();
                worksheet.Column(6).AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Pedidos "+cliente.NombreCompleto+".xlsx");
                }
            }

        }


    }
}
