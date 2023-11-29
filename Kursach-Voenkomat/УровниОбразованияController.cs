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
    public class УровниОбразованияController : Controller
    {
        private readonly ApplicationDbContext _context;

        public УровниОбразованияController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: УровниОбразования
        public async Task<IActionResult> Index()
        {
              return _context.УровниОбразования != null ? 
                          View(await _context.УровниОбразования.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.УровниОбразования'  is null.");
        }

        // GET: УровниОбразования/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.УровниОбразования == null)
            {
                return NotFound();
            }

            var уровниОбразования = await _context.УровниОбразования
                .FirstOrDefaultAsync(m => m.ID_уровня_образования == id);
            if (уровниОбразования == null)
            {
                return NotFound();
            }

            return View(уровниОбразования);
        }

        // GET: УровниОбразования/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: УровниОбразования/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_уровня_образования,Наименование_уровня_образования")] УровниОбразования уровниОбразования)
        {
            if (ModelState.IsValid)
            {
                _context.Add(уровниОбразования);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(уровниОбразования);
        }

        // GET: УровниОбразования/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.УровниОбразования == null)
            {
                return NotFound();
            }

            var уровниОбразования = await _context.УровниОбразования.FindAsync(id);
            if (уровниОбразования == null)
            {
                return NotFound();
            }
            return View(уровниОбразования);
        }

        // POST: УровниОбразования/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_уровня_образования,Наименование_уровня_образования")] УровниОбразования уровниОбразования)
        {
            if (id != уровниОбразования.ID_уровня_образования)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(уровниОбразования);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!УровниОбразованияExists(уровниОбразования.ID_уровня_образования))
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
            return View(уровниОбразования);
        }

        // GET: УровниОбразования/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.УровниОбразования == null)
            {
                return NotFound();
            }

            var уровниОбразования = await _context.УровниОбразования
                .FirstOrDefaultAsync(m => m.ID_уровня_образования == id);
            if (уровниОбразования == null)
            {
                return NotFound();
            }

            return View(уровниОбразования);
        }

        // POST: УровниОбразования/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.УровниОбразования == null)
            {
                return Problem("Entity set 'ApplicationDbContext.УровниОбразования'  is null.");
            }
            var уровниОбразования = await _context.УровниОбразования.FindAsync(id);
            if (уровниОбразования != null)
            {
                _context.УровниОбразования.Remove(уровниОбразования);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool УровниОбразованияExists(int id)
        {
          return (_context.УровниОбразования?.Any(e => e.ID_уровня_образования == id)).GetValueOrDefault();
        }
    }
}
