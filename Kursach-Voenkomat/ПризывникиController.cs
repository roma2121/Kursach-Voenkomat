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
    public class ПризывникиController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ПризывникиController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Призывники
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Призывники.Include(п => п.Пол);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Призывники/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Призывники == null)
            {
                return NotFound();
            }

            var призывники = await _context.Призывники
                .Include(п => п.Пол)
                .FirstOrDefaultAsync(m => m.ID_призывника == id);
            if (призывники == null)
            {
                return NotFound();
            }

            return View(призывники);
        }

        // GET: Призывники/Create
        public IActionResult Create()
        {
            var полыList = _context.Полы.Select(п => new { ID_пола = п.ID_пола, Наименование = п.Пол }).ToList();

            ViewData["ID_пола"] = new SelectList(полыList, "ID_пола", "Наименование");

            return View();
        }

        // POST: Призывники/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_призывника,Имя,Фамилия,Отчество,Дата_рождения,Номер_телефона,ID_пола")] Призывники призывники)
        {
            if (ModelState.IsValid)
            {
                _context.Add(призывники);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var полыList = _context.Полы.Select(п => new { ID_пола = п.ID_пола, Наименование = п.Пол }).ToList();
            ViewData["ID_пола"] = new SelectList(полыList, "ID_пола", "Наименование", призывники.ID_пола);

            return View(призывники);
        }

        // GET: Призывники/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Призывники == null)
            {
                return NotFound();
            }

            var призывники = await _context.Призывники.FindAsync(id);
            if (призывники == null)
            {
                return NotFound();
            }

            var полыList = _context.Полы.Select(п => new { ID_пола = п.ID_пола, Наименование = п.Пол }).ToList();
            ViewData["ID_пола"] = new SelectList(полыList, "ID_пола", "Наименование", призывники.ID_пола);

            return View(призывники);
        }

        // POST: Призывники/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_призывника,Имя,Фамилия,Отчество,Дата_рождения,Номер_телефона,ID_пола")] Призывники призывники)
        {
            if (id != призывники.ID_призывника)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(призывники);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПризывникиExists(призывники.ID_призывника))
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

            var полыList = _context.Полы.Select(п => new { ID_пола = п.ID_пола, Наименование = п.Пол }).ToList();
            ViewData["ID_пола"] = new SelectList(полыList, "ID_пола", "Наименование", призывники.ID_пола);

            return View(призывники);
        }

        // GET: Призывники/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Призывники == null)
            {
                return NotFound();
            }

            var призывники = await _context.Призывники
                .Include(п => п.Пол)
                .FirstOrDefaultAsync(m => m.ID_призывника == id);
            if (призывники == null)
            {
                return NotFound();
            }

            return View(призывники);
        }

        // POST: Призывники/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Призывники == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Призывники'  is null.");
            }
            var призывники = await _context.Призывники.FindAsync(id);
            if (призывники != null)
            {
                _context.Призывники.Remove(призывники);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПризывникиExists(int id)
        {
          return (_context.Призывники?.Any(e => e.ID_призывника == id)).GetValueOrDefault();
        }
    }
}
