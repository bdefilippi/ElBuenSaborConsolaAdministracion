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
    public class RubroArticulosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RubroArticulosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RubroArticulos
        public async Task<IActionResult> Index(string searchString)
        {
            var articulos = await _context.RubrosArticulos.Where(a => a.Disabled.Equals(false)).ToListAsync();

            return View(articulos);
        }

        // GET: RubroArticulos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubroArticulo = await _context.RubrosArticulos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rubroArticulo == null)
            {
                return NotFound();
            }

            return View(rubroArticulo);
        }

        // GET: RubroArticulos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RubroArticulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Denominacion,Disabled")] RubroArticulo rubroArticulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rubroArticulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rubroArticulo);
        }

        // GET: RubroArticulos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubroArticulo = await _context.RubrosArticulos.FindAsync(id);
            if (rubroArticulo == null)
            {
                return NotFound();
            }
            return View(rubroArticulo);
        }

        // POST: RubroArticulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Denominacion,Disabled")] RubroArticulo rubroArticulo)
        {
            if (id != rubroArticulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rubroArticulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RubroArticuloExists(rubroArticulo.Id))
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
            return View(rubroArticulo);
        }

        // GET: RubroArticulos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubroArticulo = await _context.RubrosArticulos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rubroArticulo == null)
            {
                return NotFound();
            }

            return View(rubroArticulo);
        }

        // POST: RubroArticulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var rubroArticulo = await _context.RubrosArticulos.FindAsync(id);

            if (id != rubroArticulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rubroArticulo.Disabled = true;
                    _context.Update(rubroArticulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RubroArticuloExists(rubroArticulo.Id))
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
            return View(rubroArticulo);
        }

        private bool RubroArticuloExists(long id)
        {
            return _context.RubrosArticulos.Any(e => e.Id == id);
        }
    }
}
