using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;

namespace Kursach_Voenkomat
{
    public class КатегорииПрофессиональнойПригодностиController : Controller
    {
        private readonly ApplicationDbContext _context;

        public КатегорииПрофессиональнойПригодностиController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: КатегорииПрофессиональнойПригодности
        public async Task<IActionResult> Index()
        {
              return _context.КатегорииПрофессиональнойПригодности != null ? 
                          View(await _context.КатегорииПрофессиональнойПригодности.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.КатегорииПрофессиональнойПригодности'  is null.");
        }

        // GET: КатегорииПрофессиональнойПригодности/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.КатегорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            var категорииПрофессиональнойПригодности = await _context.КатегорииПрофессиональнойПригодности
                .FirstOrDefaultAsync(m => m.ID_категории_профессиональной_пригодности == id);
            if (категорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            return View(категорииПрофессиональнойПригодности);
        }

        // GET: КатегорииПрофессиональнойПригодности/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: КатегорииПрофессиональнойПригодности/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_категории_профессиональной_пригодности,Категории_профессиональной_пригодности")] КатегорииПрофессиональнойПригодности категорииПрофессиональнойПригодности)
        {
            if (ModelState.IsValid)
            {
                _context.Add(категорииПрофессиональнойПригодности);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(категорииПрофессиональнойПригодности);
        }

        // GET: КатегорииПрофессиональнойПригодности/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.КатегорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            var категорииПрофессиональнойПригодности = await _context.КатегорииПрофессиональнойПригодности.FindAsync(id);
            if (категорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }
            return View(категорииПрофессиональнойПригодности);
        }

        // POST: КатегорииПрофессиональнойПригодности/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_категории_профессиональной_пригодности,Категории_профессиональной_пригодности")] КатегорииПрофессиональнойПригодности категорииПрофессиональнойПригодности)
        {
            if (id != категорииПрофессиональнойПригодности.ID_категории_профессиональной_пригодности)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(категорииПрофессиональнойПригодности);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!КатегорииПрофессиональнойПригодностиExists(категорииПрофессиональнойПригодности.ID_категории_профессиональной_пригодности))
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
            return View(категорииПрофессиональнойПригодности);
        }

        // GET: КатегорииПрофессиональнойПригодности/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.КатегорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            var категорииПрофессиональнойПригодности = await _context.КатегорииПрофессиональнойПригодности
                .FirstOrDefaultAsync(m => m.ID_категории_профессиональной_пригодности == id);
            if (категорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            return View(категорииПрофессиональнойПригодности);
        }

        // POST: КатегорииПрофессиональнойПригодности/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.КатегорииПрофессиональнойПригодности == null)
            {
                return Problem("Entity set 'ApplicationDbContext.КатегорииПрофессиональнойПригодности'  is null.");
            }
            var категорииПрофессиональнойПригодности = await _context.КатегорииПрофессиональнойПригодности.FindAsync(id);
            if (категорииПрофессиональнойПригодности != null)
            {
                _context.КатегорииПрофессиональнойПригодности.Remove(категорииПрофессиональнойПригодности);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool КатегорииПрофессиональнойПригодностиExists(int id)
        {
          return (_context.КатегорииПрофессиональнойПригодности?.Any(e => e.ID_категории_профессиональной_пригодности == id)).GetValueOrDefault();
        }
    }
}
