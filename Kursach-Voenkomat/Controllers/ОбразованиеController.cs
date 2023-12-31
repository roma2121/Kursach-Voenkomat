﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kursach_Voenkomat
{
    public class ОбразованиеController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ОбразованиеController(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied"); // Отображение страницы с сообщением об отказе в доступе
        }

        // GET: Образование
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Образование");

            var applicationDbContext = _context.Образование.Include(о => о.Призывник).Include(о => о.образовательнаяОрганизация).Include(о => о.уровеньОбразования);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Образование/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "Образование");

            if (id == null || _context.Образование == null)
            {
                return NotFound();
            }

            var образование = await _context.Образование
                .Include(о => о.Призывник)
                .Include(о => о.образовательнаяОрганизация)
                .Include(о => о.уровеньОбразования)
                .FirstOrDefaultAsync(m => m.ID_записи_об_образовании == id);
            if (образование == null)
            {
                return NotFound();
            }

            return View(образование);
        }

        // GET: Образование/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Образование");

            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            ViewData["ID_образовательной_организации"] = new SelectList(_context.ОбразовательныеОрганизации, "ID_образовательной_организации", "Наименование_образовательной_организации");
            ViewData["ID_уровня_образования"] = new SelectList(_context.УровниОбразования, "ID_уровня_образования", "Наименование_уровня_образования");
            return View();
        }

        // POST: Образование/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_записи_об_образовании,ID_призывника,Номер_документа_об_образовании,ID_уровня_образования,Специальность,Дата_окончания,ID_образовательной_организации")] Образование образование)
        {
            if (ModelState.IsValid)
            {
                _context.Add(образование);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", образование.ID_призывника);
            ViewData["ID_образовательной_организации"] = new SelectList(_context.ОбразовательныеОрганизации, "ID_образовательной_организации", "Наименование_образовательной_организации", образование.ID_образовательной_организации);
            ViewData["ID_уровня_образования"] = new SelectList(_context.УровниОбразования, "ID_уровня_образования", "Наименование_уровня_образования", образование.ID_уровня_образования);
            return View(образование);
        }

        // GET: Образование/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Образование");

            if (id == null || _context.Образование == null)
            {
                return NotFound();
            }

            var образование = await _context.Образование.FindAsync(id);
            if (образование == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", образование.ID_призывника);
            ViewData["ID_образовательной_организации"] = new SelectList(_context.ОбразовательныеОрганизации, "ID_образовательной_организации", "Наименование_образовательной_организации", образование.ID_образовательной_организации);
            ViewData["ID_уровня_образования"] = new SelectList(_context.УровниОбразования, "ID_уровня_образования", "Наименование_уровня_образования", образование.ID_уровня_образования);
            return View(образование);
        }

        // POST: Образование/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_записи_об_образовании,ID_призывника,Номер_документа_об_образовании,ID_уровня_образования,Специальность,Дата_окончания,ID_образовательной_организации")] Образование образование)
        {
            if (id != образование.ID_записи_об_образовании)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(образование);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ОбразованиеExists(образование.ID_записи_об_образовании))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", образование.ID_призывника);
            ViewData["ID_образовательной_организации"] = new SelectList(_context.ОбразовательныеОрганизации, "ID_образовательной_организации", "Наименование_образовательной_организации", образование.ID_образовательной_организации);
            ViewData["ID_уровня_образования"] = new SelectList(_context.УровниОбразования, "ID_уровня_образования", "ID_уровня_образования", образование.ID_уровня_образования);
            return View(образование);
        }

        // GET: Образование/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Образование");

            if (id == null || _context.Образование == null)
            {
                return NotFound();
            }

            var образование = await _context.Образование
                .Include(о => о.Призывник)
                .Include(о => о.образовательнаяОрганизация)
                .Include(о => о.уровеньОбразования)
                .FirstOrDefaultAsync(m => m.ID_записи_об_образовании == id);
            if (образование == null)
            {
                return NotFound();
            }

            return View(образование);
        }

        // POST: Образование/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Образование == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Образование'  is null.");
            }
            var образование = await _context.Образование.FindAsync(id);
            if (образование != null)
            {
                _context.Образование.Remove(образование);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ОбразованиеExists(int id)
        {
          return (_context.Образование?.Any(e => e.ID_записи_об_образовании == id)).GetValueOrDefault();
        }
    }
}
