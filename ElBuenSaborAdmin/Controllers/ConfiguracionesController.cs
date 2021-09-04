using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElBuenSaborAdmin.Data;
using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElBuenSaborAdmin.Controllers
{
    [Authorize]
    public class ConfiguracionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Configuraciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Configuraciones.Where(a => a.Disabled.Equals(false)).ToListAsync());
        }

        // GET: Configuraciones/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracion = await _context.Configuraciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (configuracion == null)
            {
                return NotFound();
            }

            return View(configuracion);
        }

        // GET: Configuraciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Configuraciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CantidadCocineros,EmailEmpresa,TokenMercadoPago,Disabled")] Configuracion configuracion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(configuracion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(configuracion);
        }

        // GET: Configuraciones/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracion = await _context.Configuraciones.FindAsync(id);
            if (configuracion == null)
            {
                return NotFound();
            }
            return View(configuracion);
        }

        // POST: Configuraciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CantidadCocineros,EmailEmpresa,TokenMercadoPago,Disabled")] Configuracion configuracion)
        {
            if (id != configuracion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(configuracion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfiguracionExists(configuracion.Id))
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
            return View(configuracion);
        }

        // GET: Configuraciones/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracion = await _context.Configuraciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (configuracion == null)
            {
                return NotFound();
            }

            return View(configuracion);
        }

        // POST: Configuraciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var configuracion = await _context.Configuraciones.FindAsync(id);

            if (id != configuracion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    configuracion.Disabled = true;
                    _context.Update(configuracion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfiguracionExists(configuracion.Id))
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
            return View(configuracion);
        }

        private bool ConfiguracionExists(long id)
        {
            return _context.Configuraciones.Any(e => e.Id == id);
        }
    }
}
