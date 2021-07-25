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
    public class DomiciliosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DomiciliosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Domicilios
        public async Task<IActionResult> Index(long? id)
        {
            var domVM = new IndexDomicilioVM
            {
                Domicilios = await _context.Domicilios.Where(a => a.Disabled.Equals(false)).Include(d => d.Cliente).Where(a => a.Disabled.Equals(false)).Where(d => d.ClienteID == id).ToListAsync(),
                ClienteID = id
            };
            //var applicationDbContext = _context.Domicilios.Where(a => a.Disabled.Equals(false)).Include(d => d.Cliente).Where(a => a.Disabled.Equals(false));
            //return View(await applicationDbContext.ToListAsync());
            return View(domVM);
        }

        // GET: Domicilios/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domicilio = await _context.Domicilios
                .Include(d => d.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (domicilio == null)
            {
                return NotFound();
            }

            return View(domicilio);
        }

        // GET: Domicilios/Create
        public IActionResult Create(long? id)
        {
            var domicilioVM = new CrearDomicilioVM
            {
                ClienteID = id
            };

            ViewData["ClienteID"] = new SelectList(_context.Clientes.Where(r => r.Disabled.Equals(false)), "Id", "NombreCompleto");
            return View(domicilioVM);
        }

        // POST: Domicilios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearDomicilioVM crearDomicilioVM)
        {
            if (ModelState.IsValid)
            {
                var domicilio = new Domicilio
                {
                    Calle = crearDomicilioVM.Calle,
                    Localidad = crearDomicilioVM.Localidad,
                    Numero = crearDomicilioVM.Numero,
                    ClienteID = (long)crearDomicilioVM.ClienteID
                };
                _context.Add(domicilio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = domicilio.ClienteID });
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes.Where(r => r.Disabled.Equals(false)), "Id", "NombreCompleto", crearDomicilioVM.ClienteID);
            return View(crearDomicilioVM);
        }

        // GET: Domicilios/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domicilio = await _context.Domicilios.FindAsync(id);
            if (domicilio == null)
            {
                return NotFound();
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes.Where(r => r.Disabled.Equals(false)), "Id", "NombreCompleto", domicilio.ClienteID);
            return View(domicilio);
        }

        // POST: Domicilios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Calle,Numero,Localidad,Disabled,ClienteID")] Domicilio domicilio)
        {
            if (id != domicilio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domicilio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomicilioExists(domicilio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = domicilio.ClienteID });
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes.Where(r => r.Disabled.Equals(false)), "Id", "NombreCompleto", domicilio.ClienteID);
            return View(domicilio);
        }

        // GET: Domicilios/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domicilio = await _context.Domicilios
                .Include(d => d.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (domicilio == null)
            {
                return NotFound();
            }

            return View(domicilio);
        }

        // POST: Domicilios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var domicilio = await _context.Domicilios.FindAsync(id);

            if (id != domicilio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    domicilio.Disabled = true;
                    _context.Update(domicilio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomicilioExists(domicilio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = domicilio.ClienteID });
            }
            return View(domicilio);
        }

        private bool DomicilioExists(long id)
        {
            return _context.Domicilios.Any(e => e.Id == id);
        }
    }
}
