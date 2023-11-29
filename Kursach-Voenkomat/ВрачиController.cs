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
    public class ВрачиController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ВрачиController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Врачи
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Врачи.Include(в => в.Специальность);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Врачи/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Врачи == null)
            {
                return NotFound();
            }

            var врачи = await _context.Врачи
                .Include(в => в.Специальность)
                .FirstOrDefaultAsync(m => m.ID_врача == id);
            if (врачи == null)
            {
                return NotFound();
            }

            return View(врачи);
        }

        // GET: Врачи/Create
        public IActionResult Create()
        {
            ViewData["ID_специальности"] = new SelectList(_context.СпециальностиВрачей, "ID_специальности", "Наименование_специальности");
            return View();
        }

        // POST: Врачи/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_врача,Фамилия,Имя,Отчество,ID_специальности")] Врачи врачи)
        {
            if (ModelState.IsValid)
            {
                _context.Add(врачи);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_специальности"] = new SelectList(_context.СпециальностиВрачей, "ID_специальности", "ID_специальности", врачи.ID_специальности);
            return View(врачи);
        }

        // GET: Врачи/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Врачи == null)
            {
                return NotFound();
            }

            var врачи = await _context.Врачи.FindAsync(id);
            if (врачи == null)
            {
                return NotFound();
            }
            ViewData["ID_специальности"] = new SelectList(_context.СпециальностиВрачей, "ID_специальности", "Наименование_специальности", врачи.ID_специальности);
            return View(врачи);
        }

        // POST: Врачи/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_врача,Фамилия,Имя,Отчество,ID_специальности")] Врачи врачи)
        {
            if (id != врачи.ID_врача)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(врачи);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВрачиExists(врачи.ID_врача))
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
            ViewData["ID_специальности"] = new SelectList(_context.СпециальностиВрачей, "ID_специальности", "ID_специальности", врачи.ID_специальности);
            return View(врачи);
        }

        // GET: Врачи/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Врачи == null)
            {
                return NotFound();
            }

            var врачи = await _context.Врачи
                .Include(в => в.Специальность)
                .FirstOrDefaultAsync(m => m.ID_врача == id);
            if (врачи == null)
            {
                return NotFound();
            }

            return View(врачи);
        }

        // POST: Врачи/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Врачи == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Врачи'  is null.");
            }
            var врачи = await _context.Врачи.FindAsync(id);
            if (врачи != null)
            {
                _context.Врачи.Remove(врачи);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВрачиExists(int id)
        {
          return (_context.Врачи?.Any(e => e.ID_врача == id)).GetValueOrDefault();
        }
    }
}
