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
    public class ВоенноПрофессиональныеНаправленностиController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ВоенноПрофессиональныеНаправленностиController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ВоенноПрофессиональныеНаправленности
        public async Task<IActionResult> Index()
        {
              return _context.ВоенноПрофессиональныеНаправленности != null ? 
                          View(await _context.ВоенноПрофессиональныеНаправленности.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ВоенноПрофессиональныеНаправленности'  is null.");
        }

        // GET: ВоенноПрофессиональныеНаправленности/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ВоенноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            var военноПрофессиональныеНаправленности = await _context.ВоенноПрофессиональныеНаправленности
                .FirstOrDefaultAsync(m => m.ID_военно_профессиональной_направленности == id);
            if (военноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            return View(военноПрофессиональныеНаправленности);
        }

        // GET: ВоенноПрофессиональныеНаправленности/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ВоенноПрофессиональныеНаправленности/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_военно_профессиональной_направленности,Наименование_военно_профессиональной_направленности")] ВоенноПрофессиональныеНаправленности военноПрофессиональныеНаправленности)
        {
            if (ModelState.IsValid)
            {
                _context.Add(военноПрофессиональныеНаправленности);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(военноПрофессиональныеНаправленности);
        }

        // GET: ВоенноПрофессиональныеНаправленности/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ВоенноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            var военноПрофессиональныеНаправленности = await _context.ВоенноПрофессиональныеНаправленности.FindAsync(id);
            if (военноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }
            return View(военноПрофессиональныеНаправленности);
        }

        // POST: ВоенноПрофессиональныеНаправленности/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_военно_профессиональной_направленности,Наименование_военно_профессиональной_направленности")] ВоенноПрофессиональныеНаправленности военноПрофессиональныеНаправленности)
        {
            if (id != военноПрофессиональныеНаправленности.ID_военно_профессиональной_направленности)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(военноПрофессиональныеНаправленности);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВоенноПрофессиональныеНаправленностиExists(военноПрофессиональныеНаправленности.ID_военно_профессиональной_направленности))
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
            return View(военноПрофессиональныеНаправленности);
        }

        // GET: ВоенноПрофессиональныеНаправленности/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ВоенноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            var военноПрофессиональныеНаправленности = await _context.ВоенноПрофессиональныеНаправленности
                .FirstOrDefaultAsync(m => m.ID_военно_профессиональной_направленности == id);
            if (военноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            return View(военноПрофессиональныеНаправленности);
        }

        // POST: ВоенноПрофессиональныеНаправленности/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ВоенноПрофессиональныеНаправленности == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ВоенноПрофессиональныеНаправленности'  is null.");
            }
            var военноПрофессиональныеНаправленности = await _context.ВоенноПрофессиональныеНаправленности.FindAsync(id);
            if (военноПрофессиональныеНаправленности != null)
            {
                _context.ВоенноПрофессиональныеНаправленности.Remove(военноПрофессиональныеНаправленности);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВоенноПрофессиональныеНаправленностиExists(int id)
        {
          return (_context.ВоенноПрофессиональныеНаправленности?.Any(e => e.ID_военно_профессиональной_направленности == id)).GetValueOrDefault();
        }
    }
}
