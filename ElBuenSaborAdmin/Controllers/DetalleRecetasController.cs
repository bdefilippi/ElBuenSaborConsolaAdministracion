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
    public class DetalleRecetasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetalleRecetasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DetalleRecetas
        public async Task<IActionResult> Index(long? idArt, long? idRec)
        {
            var detRecVM = new IndexDetalleRecetaVM
            {
                DetalleRecetas = await _context.DetallesRecetas.Where(a => a.Disabled.Equals(false))
                .Include(d => d.Articulo).ThenInclude(a => a.Stocks).Where(a => a.Disabled.Equals(false))
                .Include(d => d.Receta).Where(a => a.Disabled.Equals(false))
                .Where(r => r.RecetaID == idRec).ToListAsync(),
                IdArticulo = idArt,
                IdReceta = idRec
            };

           
            return View(detRecVM);
        }

        // GET: DetalleRecetas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleReceta = await _context.DetallesRecetas
                .Include(d => d.Articulo)
                .Include(d => d.Receta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleReceta == null)
            {
                return NotFound();
            }

            return View(detalleReceta);
        }

        // GET: DetalleRecetas/Create
        public IActionResult Create(long? idArt, long? idRec)
        {
            var detRectVM = new CrearDetalleRecetaVM
            {
                IdArticulo = idArt,
                IdReceta = idRec
            };

            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "GetDenominacionConUnidad");
            ViewData["RecetaID"] = new SelectList(_context.Recetas.Where(r => r.Disabled.Equals(false)), "Id", "Id");
            return View(detRectVM);
        }

        // POST: DetalleRecetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearDetalleRecetaVM crearDetalleRecetaVM)
        {
     
            if (ModelState.IsValid)
            {
                var detalleReceta = new DetalleReceta
                {
                    Cantidad = crearDetalleRecetaVM.Cantidad,
                    ArticuloID = (long)crearDetalleRecetaVM.IdArticulo,
                    RecetaID = (long)crearDetalleRecetaVM.IdReceta
                };

                _context.Add(detalleReceta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { idArt = detalleReceta.ArticuloID, idRec = detalleReceta.RecetaID });
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "GetDenominacionConUnidad", crearDetalleRecetaVM.IdArticulo);
            ViewData["RecetaID"] = new SelectList(_context.Recetas.Where(r => r.Disabled.Equals(false)), "Id", "Id", crearDetalleRecetaVM.IdReceta);
            return View(crearDetalleRecetaVM);
        }

        // GET: DetalleRecetas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleReceta = await _context.DetallesRecetas.FindAsync(id);
            if (detalleReceta == null)
            {
                return NotFound();
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "GetDenominacionConUnidad", detalleReceta.ArticuloID);
            ViewData["RecetaID"] = new SelectList(_context.Recetas.Where(r => r.Disabled.Equals(false)), "Id", "Id", detalleReceta.RecetaID);
            return View(detalleReceta);
        }

        // POST: DetalleRecetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Cantidad,ArticuloID,RecetaID")] DetalleReceta detalleReceta)
        {
            if (id != detalleReceta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleReceta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleRecetaExists(detalleReceta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { idArt = detalleReceta.ArticuloID, idRec = detalleReceta.RecetaID });
            }
            ViewData["ArticuloID"] = new SelectList(_context.Articulos.Where(r => r.Disabled.Equals(false)), "Id", "GetDenominacionConUnidad", detalleReceta.ArticuloID);
            ViewData["RecetaID"] = new SelectList(_context.Recetas.Where(r => r.Disabled.Equals(false)), "Id", "Id", detalleReceta.RecetaID);
            return View(detalleReceta);
        }

        // GET: DetalleRecetas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleReceta = await _context.DetallesRecetas
                .Include(d => d.Articulo)
                .Include(d => d.Receta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleReceta == null)
            {
                return NotFound();
            }

            return View(detalleReceta);
        }

        // POST: DetalleRecetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var detalleReceta = await _context.DetallesRecetas.FindAsync(id);

            if (id != detalleReceta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    detalleReceta.Disabled = true;
                    _context.Update(detalleReceta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleRecetaExists(detalleReceta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { idArt = detalleReceta.ArticuloID, idRec = detalleReceta.RecetaID });
            }
            return View(detalleReceta);
        }

        private bool DetalleRecetaExists(long id)
        {
            return _context.DetallesRecetas.Any(e => e.Id == id);
        }
    }
}
