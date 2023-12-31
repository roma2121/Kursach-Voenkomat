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
    public class ЗаключенияВрачейController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ЗаключенияВрачейController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: ЗаключенияВрачей
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ЗаключенияВрачей");

            var applicationDbContext = _context.ЗаключенияВрачей.Include(з => з.Врач).Include(з => з.Категория).Include(з => з.Призывник);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ЗаключенияВрачей/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "ЗаключенияВрачей");

            if (id == null || _context.ЗаключенияВрачей == null)
            {
                return NotFound();
            }

            var заключенияВрачей = await _context.ЗаключенияВрачей
                .Include(з => з.Врач)
                .Include(з => з.Категория)
                .Include(з => з.Призывник)
                .FirstOrDefaultAsync(m => m.ID_заключения == id);
            if (заключенияВрачей == null)
            {
                return NotFound();
            }

            return View(заключенияВрачей);
        }

        // GET: ЗаключенияВрачей/Create
        [Authorize(Roles = "Medical_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ЗаключенияВрачей");

            ViewData["ID_врача"] = new SelectList(_context.Врачи, "ID_врача", "ВрачФИО");
            ViewData["ID_категории_годности"] = new SelectList(_context.КатегорииГодности, "ID_категории_годности", "Наименование_категории");
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            return View();
        }

        // POST: ЗаключенияВрачей/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_заключения,ID_призывника,Дата_выписки_заключения,ID_врача,Предварительный_диагноз,ID_категории_годности,Заключение")] ЗаключенияВрачей заключенияВрачей)
        {
            if (ModelState.IsValid)
            {
                _context.Add(заключенияВрачей);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_врача"] = new SelectList(_context.Врачи, "ID_врача", "ID_врача", заключенияВрачей.ID_врача);
            ViewData["ID_категории_годности"] = new SelectList(_context.КатегорииГодности, "ID_категории_годности", "ID_категории_годности", заключенияВрачей.ID_категории_годности);
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", заключенияВрачей.ID_призывника);
            return View(заключенияВрачей);
        }

        // GET: ЗаключенияВрачей/Edit/5
        [Authorize(Roles = "Medical_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ЗаключенияВрачей");

            if (id == null || _context.ЗаключенияВрачей == null)
            {
                return NotFound();
            }

            var заключенияВрачей = await _context.ЗаключенияВрачей.FindAsync(id);
            if (заключенияВрачей == null)
            {
                return NotFound();
            }
            ViewData["ID_врача"] = new SelectList(_context.Врачи, "ID_врача", "ВрачФИО", заключенияВрачей.ID_врача);
            ViewData["ID_категории_годности"] = new SelectList(_context.КатегорииГодности, "ID_категории_годности", "Наименование_категории", заключенияВрачей.ID_категории_годности);
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", заключенияВрачей.ID_призывника);
            return View(заключенияВрачей);
        }

        // POST: ЗаключенияВрачей/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_заключения,ID_призывника,Дата_выписки_заключения,ID_врача,Предварительный_диагноз,ID_категории_годности,Заключение")] ЗаключенияВрачей заключенияВрачей)
        {
            if (id != заключенияВрачей.ID_заключения)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(заключенияВрачей);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ЗаключенияВрачейExists(заключенияВрачей.ID_заключения))
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
            ViewData["ID_врача"] = new SelectList(_context.Врачи, "ID_врача", "ID_врача", заключенияВрачей.ID_врача);
            ViewData["ID_категории_годности"] = new SelectList(_context.КатегорииГодности, "ID_категории_годности", "ID_категории_годности", заключенияВрачей.ID_категории_годности);
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", заключенияВрачей.ID_призывника);
            return View(заключенияВрачей);
        }

        // GET: ЗаключенияВрачей/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ЗаключенияВрачей");

            if (id == null || _context.ЗаключенияВрачей == null)
            {
                return NotFound();
            }

            var заключенияВрачей = await _context.ЗаключенияВрачей
                .Include(з => з.Врач)
                .Include(з => з.Категория)
                .Include(з => з.Призывник)
                .FirstOrDefaultAsync(m => m.ID_заключения == id);
            if (заключенияВрачей == null)
            {
                return NotFound();
            }

            return View(заключенияВрачей);
        }

        // POST: ЗаключенияВрачей/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ЗаключенияВрачей == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ЗаключенияВрачей'  is null.");
            }
            var заключенияВрачей = await _context.ЗаключенияВрачей.FindAsync(id);
            if (заключенияВрачей != null)
            {
                _context.ЗаключенияВрачей.Remove(заключенияВрачей);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ЗаключенияВрачейExists(int id)
        {
          return (_context.ЗаключенияВрачей?.Any(e => e.ID_заключения == id)).GetValueOrDefault();
        }
    }
}
