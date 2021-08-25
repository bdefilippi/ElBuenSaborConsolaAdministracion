using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElBuenSaborAdmin.Data;
using ElBuenSaborAdmin.Models;
using ClosedXML.Excel;
using System.IO;
using ElBuenSaborAdmin.Viewmodels;
using Microsoft.Data.SqlClient;

namespace ElBuenSaborAdmin.Controllers
{
    public class FacturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Facturas
        public async Task<IActionResult> Index() 
        {
            var applicationDbContext = _context.Facturas.Where(a => a.Disabled.Equals(false))
                .Include(f => f.DetallesFactura).Where(a => a.Disabled.Equals(false))
                .Include(f => f.Pedido).ThenInclude(p => p.DetallesPedido).ThenInclude(d => d.Articulo).ThenInclude(a => a.PreciosVentaArticulos).Where(a => a.Disabled.Equals(false))
                .Include(f => f.Pedido.Cliente).Where(a => a.Disabled.Equals(false));
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Facturas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // GET: Facturas/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            return View();
        }

        // POST: Facturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,Fecha,MontoDescuento,FormaPago,Disabled,PedidoId")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(factura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", factura.PedidoId);
            return View(factura);
        }

        // GET: Facturas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", factura.PedidoId);
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Numero,Fecha,MontoDescuento,FormaPago,Disabled,PedidoId")] Factura factura)
        {
            if (id != factura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(factura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaExists(factura.Id))
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
            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Where(r => r.Disabled.Equals(false)), "Id", "Id", factura.PedidoId);
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var factura = await _context.Facturas.FindAsync(id);

            if (id != factura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    factura.Disabled = true;
                    _context.Update(factura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaExists(factura.Id))
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
            return View(factura);
        }

        private bool FacturaExists(long id)
        {
            return _context.Facturas.Any(e => e.Id == id);
        }

        public IActionResult Ingresos()
        {
            var hoy = DateTime.Now;

            var IngresosVM = new IngresosVM
            {
                Fecha = new DateTime(hoy.Year, hoy.Month, hoy.Day)

            };

            return View(IngresosVM);
        }

        public async Task<IActionResult> GenerarReporteIngresos(DateTime fecha)
        {
            //Reporte de ingresos
            //Filtrado de facturas por mes y año
            var facturas = await _context.Facturas.Where(f => f.Fecha.Month == fecha.Month && f.Fecha.Year == fecha.Year).Where(r => r.Disabled.Equals(false)).OrderBy(f => f.Fecha).ToListAsync();
            
            //Create an instance of ExcelEngine
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ingresos Mensuales");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Fecha";
                worksheet.Cell(currentRow, 2).Value = "Subtotal";

                decimal total = 0;

                //ingresos mensuales
                foreach (var factura in facturas)
                {
                    currentRow++;
                    total += factura.Total;
                    worksheet.Cell(currentRow, 1).Value = factura.Fecha;
                    worksheet.Cell(currentRow, 2).Value = "$"+factura.Total;

                }

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).AdjustToContents();

                //ingresos diarios
                var fechaFactura = new DateTime();
                foreach (var factura in facturas.OrderBy(f => f.Fecha))
                {
                    if(factura.Fecha.Day != fechaFactura.Day || factura.Fecha.Month != fechaFactura.Month)
                    {
                        currentRow++;
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = "TOTAL";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "$" + total;
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;

                        fechaFactura = factura.Fecha;
                        worksheet = workbook.Worksheets.Add(fechaFactura.Day + "-" + fechaFactura.Month);

                        currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "Fecha";
                        worksheet.Cell(currentRow, 2).Value = "Subtotal";

                        total = 0;
                    }

                    currentRow++;
                    total += factura.Total;
                    worksheet.Cell(currentRow, 1).Value = factura.Fecha;
                    worksheet.Cell(currentRow, 2).Value = "$" + factura.Total;
                    worksheet.Column(1).AdjustToContents();
                    worksheet.Column(2).AdjustToContents();

                }

                currentRow++;
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = "TOTAL";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Value = "$" + total;
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;

                

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Ingresos Mes " + fecha.Month + " Año " + fecha.Year + ".xlsx");
                }
            }

        }


        public IActionResult Ganancias()
        {
            var hoy = DateTime.Now;

            var GananciasVM = new GananciasVM
            {
                FechaInicio = new DateTime(2020, 1, 1),
                FechaFinal = new DateTime(hoy.Year, hoy.Month, hoy.Day, hoy.Hour, hoy.Minute, 0)

            };

            return View(GananciasVM);
        }

        public async Task<IActionResult> GenerarReporteGanancias(DateTime fechaFinal, DateTime fechaInicial)
        {
            //Reporte de ganancias!
            //Sale a buscar el total de las facturas para ingresos, trae egresos y stock para calcular gastos
            //Filtrado por fecha
            var facturas = await _context.Facturas.Where(f => f.Fecha <= fechaFinal && f.Fecha >= fechaInicial)
                .Include(f => f.DetallesFactura).ThenInclude(d => d.EgresosArticulos).ThenInclude(e => e.Stock)
                .Where(r => r.Disabled.Equals(false)).OrderBy(f => f.Fecha).ToListAsync();

            //Create an instance of ExcelEngine
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ganancias");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Número";
                worksheet.Cell(currentRow, 2).Value = "Fecha";
                worksheet.Cell(currentRow, 3).Value = "Total";
                worksheet.Cell(currentRow, 4).Value = "Costos";

                decimal totalVentas = 0;
                decimal totalCostos = 0;

                foreach (var factura in facturas)
                {
                    currentRow++;
                    totalVentas += factura.Total;
                    totalCostos += factura.CostoTotal;
                    worksheet.Cell(currentRow, 1).Value = factura.Numero;
                    worksheet.Cell(currentRow, 2).Value = factura.Fecha;
                    worksheet.Cell(currentRow, 3).Value = "$" + factura.Total;
                    worksheet.Cell(currentRow, 4).Value = "$" + factura.CostoTotal;

                }

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).AdjustToContents();
                worksheet.Column(3).AdjustToContents();
                worksheet.Column(4).AdjustToContents();

                currentRow++;
                currentRow++;
                worksheet.Cell(currentRow, 2).Value = "TOTALES";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Value = "$" + totalVentas;
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 4).Value = "$" + totalCostos;
                worksheet.Cell(currentRow, 4).Style.Font.Bold = true;



                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Ganancias - " + fechaInicial.Day +"/"+fechaInicial.Month+"/"+fechaInicial.Year + " a "  + fechaFinal.Day + "/" + fechaFinal.Month + "/" + fechaFinal.Year + ".xlsx");
                }
            }

        }

        public IActionResult Ranking()
        {
            var hoy = DateTime.Now;

            var RankingVM = new RankingVM
            {
                FechaInicio = new DateTime(2020, 1, 1),
                FechaFinal = new DateTime(hoy.Year, hoy.Month, hoy.Day, hoy.Hour, hoy.Minute, 0)

            };

            return View(RankingVM);
        }

        public async Task<IActionResult> GenerarReporteRanking(DateTime fechaFinal, DateTime fechaInicial)
        {
            /* Reporte de ranking de comidas más compradas
             * Llamada mediante un stored procedure hermoso que armé en la DB :')
             * RankingProductos @FechaInicial, @FechaFinal
             * Devuelve una tabla ordenada de mayor a menor de la suma de las cantidades productos manufacturados encontrados entre las fechas
             */

            //Create an instance of ExcelEngine
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ranking");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Puesto";
                worksheet.Cell(currentRow, 2).Value = "Denominación";
                worksheet.Cell(currentRow, 3).Value = "Cantidad";

                //llamada al SP
                var rankings = await _context.Rankings.FromSqlInterpolated($"RankingProductos {fechaInicial}, {fechaFinal}").ToListAsync();

                foreach (var item in rankings)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                    worksheet.Cell(currentRow, 2).Value = item.Cantidad;
                    worksheet.Cell(currentRow, 3).Value = item.Denominacion;
                }

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).AdjustToContents();
                worksheet.Column(3).AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Ranking Artículos - " + fechaInicial.Day + "/" + fechaInicial.Month + "/" + fechaInicial.Year + " a " + fechaFinal.Day + "/" + fechaFinal.Month + "/" + fechaFinal.Year + ".xlsx");
                }
            }

        }
    }
}
