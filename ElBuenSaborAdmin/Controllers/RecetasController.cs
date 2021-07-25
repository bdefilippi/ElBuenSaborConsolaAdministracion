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
    public class RecetasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecetasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recetas/5
        public async Task<IActionResult> Index(long? id)
        {
            var IndexRecetaVM = new IndexRecetaVM
            {
                Recetas = await _context.Recetas.Where(a => a.Disabled.Equals(false)).Include(r => r.Articulo).Where(a => a.Disabled.Equals(false)).Where(r => r.ArticuloID == id).ToListAsync(),
                idArticulo = id
        };

            //var applicationDbContext = _context.Recetas.Where(a => a.Disabled.Equals(false)).Include(r => r.Articulo).Where(a => a.Disabled.Equals(false));
            return View(IndexRecetaVM);
        }

        // GET: Recetas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas
                .Include(r => r.Articulo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // GET: Recetas/Create
        public IActionResult Create(long? id)
        {
            var crearReceta = new CrearRecetaVM
            {
                ArticuloID = id
            };


            //ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r=>r.Disabled.Equals(false)), "Id", "Denominacion");
            return View(crearReceta);
        }

        // POST: Recetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearRecetaVM crearRecetaVM)
        {
            
            if (ModelState.IsValid)
            {
                var receta = new Receta
                {
                    TiempoEstimadoCocina = crearRecetaVM.TiempoEstimadoCocina,
                    Descripcion = crearRecetaVM.Descripcion,
                    ArticuloID = (long)crearRecetaVM.ArticuloID
                };
                _context.Add(receta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = receta.ArticuloID });
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", crearRecetaVM.ArticuloID);
            return View(crearRecetaVM);
        }

        // GET: Recetas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas.FindAsync(id);
            if (receta == null)
            {
                return NotFound();
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", receta.ArticuloID);
            return View(receta);
        }

        // POST: Recetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,TiempoEstimadoCocina,Descripcion,Disabled,ArticuloID")] Receta receta)
        {
            if (id != receta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(receta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = receta.ArticuloID });
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "Denominacion", receta.ArticuloID);
            return View(receta);
        }

        // GET: Recetas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas
                .Include(r => r.Articulo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var receta = await _context.Recetas.FindAsync(id);

            if (id != receta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    receta.Disabled = true;
                    _context.Update(receta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(receta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = receta.ArticuloID });
            }
            return View(receta);
        }

        private bool RecetaExists(long id)
        {
            return _context.Recetas.Any(e => e.Id == id);
        }
    }
}
