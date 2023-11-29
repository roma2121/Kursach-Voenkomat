﻿using System;
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
    public class ОбразовательныеОрганизацииController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ОбразовательныеОрганизацииController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ОбразовательныеОрганизации
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ОбразовательныеОрганизации.Include(о => о.типОбразовательнойОрганизации);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ОбразовательныеОрганизации/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ОбразовательныеОрганизации == null)
            {
                return NotFound();
            }

            var образовательныеОрганизации = await _context.ОбразовательныеОрганизации
                .Include(о => о.типОбразовательнойОрганизации)
                .FirstOrDefaultAsync(m => m.ID_образовательной_организации == id);
            if (образовательныеОрганизации == null)
            {
                return NotFound();
            }

            return View(образовательныеОрганизации);
        }

        // GET: ОбразовательныеОрганизации/Create
        public IActionResult Create()
        {
            ViewData["ID_типа_образовательной_организации"] = new SelectList(_context.ТипыОбразовательныхОрганизаций, "ID_типа_образовательной_организации", "Наименование_типа_образовательной_организации");
            return View();
        }

        // POST: ОбразовательныеОрганизации/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_образовательной_организации,Наименование_образовательной_организации,ID_типа_образовательной_организации,Адрес_образовательной_организации")] ОбразовательныеОрганизации образовательныеОрганизации)
        {
            if (ModelState.IsValid)
            {
                _context.Add(образовательныеОрганизации);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_типа_образовательной_организации"] = new SelectList(_context.ТипыОбразовательныхОрганизаций, "ID_типа_образовательной_организации", "ID_типа_образовательной_организации", образовательныеОрганизации.ID_типа_образовательной_организации);
            return View(образовательныеОрганизации);
        }

        // GET: ОбразовательныеОрганизации/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ОбразовательныеОрганизации == null)
            {
                return NotFound();
            }

            var образовательныеОрганизации = await _context.ОбразовательныеОрганизации.FindAsync(id);
            if (образовательныеОрганизации == null)
            {
                return NotFound();
            }
            ViewData["ID_типа_образовательной_организации"] = new SelectList(_context.ТипыОбразовательныхОрганизаций, "ID_типа_образовательной_организации", "Наименование_типа_образовательной_организации", образовательныеОрганизации.ID_типа_образовательной_организации);
            return View(образовательныеОрганизации);
        }

        // POST: ОбразовательныеОрганизации/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_образовательной_организации,Наименование_образовательной_организации,ID_типа_образовательной_организации,Адрес_образовательной_организации")] ОбразовательныеОрганизации образовательныеОрганизации)
        {
            if (id != образовательныеОрганизации.ID_образовательной_организации)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(образовательныеОрганизации);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ОбразовательныеОрганизацииExists(образовательныеОрганизации.ID_образовательной_организации))
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
            ViewData["ID_типа_образовательной_организации"] = new SelectList(_context.ТипыОбразовательныхОрганизаций, "ID_типа_образовательной_организации", "ID_типа_образовательной_организации", образовательныеОрганизации.ID_типа_образовательной_организации);
            return View(образовательныеОрганизации);
        }

        // GET: ОбразовательныеОрганизации/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ОбразовательныеОрганизации == null)
            {
                return NotFound();
            }

            var образовательныеОрганизации = await _context.ОбразовательныеОрганизации
                .Include(о => о.типОбразовательнойОрганизации)
                .FirstOrDefaultAsync(m => m.ID_образовательной_организации == id);
            if (образовательныеОрганизации == null)
            {
                return NotFound();
            }

            return View(образовательныеОрганизации);
        }

        // POST: ОбразовательныеОрганизации/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ОбразовательныеОрганизации == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ОбразовательныеОрганизации'  is null.");
            }
            var образовательныеОрганизации = await _context.ОбразовательныеОрганизации.FindAsync(id);
            if (образовательныеОрганизации != null)
            {
                _context.ОбразовательныеОрганизации.Remove(образовательныеОрганизации);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ОбразовательныеОрганизацииExists(int id)
        {
          return (_context.ОбразовательныеОрганизации?.Any(e => e.ID_образовательной_организации == id)).GetValueOrDefault();
        }
    }
}
