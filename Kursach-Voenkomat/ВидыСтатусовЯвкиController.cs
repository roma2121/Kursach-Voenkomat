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
    public class ВидыСтатусовЯвкиController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ВидыСтатусовЯвкиController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ВидыСтатусовЯвки
        public async Task<IActionResult> Index()
        {
              return _context.ВидыСтатусовЯвки != null ? 
                          View(await _context.ВидыСтатусовЯвки.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ВидыСтатусовЯвки'  is null.");
        }

        // GET: ВидыСтатусовЯвки/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ВидыСтатусовЯвки == null)
            {
                return NotFound();
            }

            var видыСтатусовЯвки = await _context.ВидыСтатусовЯвки
                .FirstOrDefaultAsync(m => m.ID_наименования_статуса_явки == id);
            if (видыСтатусовЯвки == null)
            {
                return NotFound();
            }

            return View(видыСтатусовЯвки);
        }

        // GET: ВидыСтатусовЯвки/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ВидыСтатусовЯвки/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_наименования_статуса_явки,Наименование_статуса_явки")] ВидыСтатусовЯвки видыСтатусовЯвки)
        {
            if (ModelState.IsValid)
            {
                _context.Add(видыСтатусовЯвки);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(видыСтатусовЯвки);
        }

        // GET: ВидыСтатусовЯвки/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ВидыСтатусовЯвки == null)
            {
                return NotFound();
            }

            var видыСтатусовЯвки = await _context.ВидыСтатусовЯвки.FindAsync(id);
            if (видыСтатусовЯвки == null)
            {
                return NotFound();
            }
            return View(видыСтатусовЯвки);
        }

        // POST: ВидыСтатусовЯвки/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_наименования_статуса_явки,Наименование_статуса_явки")] ВидыСтатусовЯвки видыСтатусовЯвки)
        {
            if (id != видыСтатусовЯвки.ID_наименования_статуса_явки)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(видыСтатусовЯвки);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВидыСтатусовЯвкиExists(видыСтатусовЯвки.ID_наименования_статуса_явки))
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
            return View(видыСтатусовЯвки);
        }

        // GET: ВидыСтатусовЯвки/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ВидыСтатусовЯвки == null)
            {
                return NotFound();
            }

            var видыСтатусовЯвки = await _context.ВидыСтатусовЯвки
                .FirstOrDefaultAsync(m => m.ID_наименования_статуса_явки == id);
            if (видыСтатусовЯвки == null)
            {
                return NotFound();
            }

            return View(видыСтатусовЯвки);
        }

        // POST: ВидыСтатусовЯвки/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ВидыСтатусовЯвки == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ВидыСтатусовЯвки'  is null.");
            }
            var видыСтатусовЯвки = await _context.ВидыСтатусовЯвки.FindAsync(id);
            if (видыСтатусовЯвки != null)
            {
                _context.ВидыСтатусовЯвки.Remove(видыСтатусовЯвки);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВидыСтатусовЯвкиExists(int id)
        {
          return (_context.ВидыСтатусовЯвки?.Any(e => e.ID_наименования_статуса_явки == id)).GetValueOrDefault();
        }
    }
}
