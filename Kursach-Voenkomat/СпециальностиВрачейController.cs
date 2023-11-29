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
    public class СпециальностиВрачейController : Controller
    {
        private readonly ApplicationDbContext _context;

        public СпециальностиВрачейController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: СпециальностиВрачей
        public async Task<IActionResult> Index()
        {
              return _context.СпециальностиВрачей != null ? 
                          View(await _context.СпециальностиВрачей.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.СпециальностиВрачей'  is null.");
        }

        // GET: СпециальностиВрачей/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.СпециальностиВрачей == null)
            {
                return NotFound();
            }

            var специальностиВрачей = await _context.СпециальностиВрачей
                .FirstOrDefaultAsync(m => m.ID_специальности == id);
            if (специальностиВрачей == null)
            {
                return NotFound();
            }

            return View(специальностиВрачей);
        }

        // GET: СпециальностиВрачей/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: СпециальностиВрачей/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_специальности,Наименование_специальности")] СпециальностиВрачей специальностиВрачей)
        {
            if (ModelState.IsValid)
            {
                _context.Add(специальностиВрачей);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(специальностиВрачей);
        }

        // GET: СпециальностиВрачей/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.СпециальностиВрачей == null)
            {
                return NotFound();
            }

            var специальностиВрачей = await _context.СпециальностиВрачей.FindAsync(id);
            if (специальностиВрачей == null)
            {
                return NotFound();
            }
            return View(специальностиВрачей);
        }

        // POST: СпециальностиВрачей/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_специальности,Наименование_специальности")] СпециальностиВрачей специальностиВрачей)
        {
            if (id != специальностиВрачей.ID_специальности)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(специальностиВрачей);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!СпециальностиВрачейExists(специальностиВрачей.ID_специальности))
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
            return View(специальностиВрачей);
        }

        // GET: СпециальностиВрачей/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.СпециальностиВрачей == null)
            {
                return NotFound();
            }

            var специальностиВрачей = await _context.СпециальностиВрачей
                .FirstOrDefaultAsync(m => m.ID_специальности == id);
            if (специальностиВрачей == null)
            {
                return NotFound();
            }

            return View(специальностиВрачей);
        }

        // POST: СпециальностиВрачей/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.СпециальностиВрачей == null)
            {
                return Problem("Entity set 'ApplicationDbContext.СпециальностиВрачей'  is null.");
            }
            var специальностиВрачей = await _context.СпециальностиВрачей.FindAsync(id);
            if (специальностиВрачей != null)
            {
                _context.СпециальностиВрачей.Remove(специальностиВрачей);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool СпециальностиВрачейExists(int id)
        {
          return (_context.СпециальностиВрачей?.Any(e => e.ID_специальности == id)).GetValueOrDefault();
        }
    }
}
