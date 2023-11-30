using System;
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
    public class ПаспортаController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ПаспортаController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: Паспорта
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, State_services, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Паспорта");

            var applicationDbContext = _context.Паспорта.Include(п => п.Призывник);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Паспорта/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, State_services, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "Паспорта");

            if (id == null || _context.Паспорта == null)
            {
                return NotFound();
            }

            var паспорта = await _context.Паспорта
                .Include(п => п.Призывник)
                .FirstOrDefaultAsync(m => m.ID_паспорта == id);
            if (паспорта == null)
            {
                return NotFound();
            }

            return View(паспорта);
        }

        // GET: Паспорта/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Паспорта");

            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            return View();
        }

        // POST: Паспорта/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_паспорта,Серия,Номер,Дата_выдачи,Кем_выдан,Место_рождения,ID_призывника,Адрес_прописки")] Паспорта паспорта)
        {
            if (ModelState.IsValid)
            {
                _context.Add(паспорта);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", паспорта.ID_призывника);
            return View(паспорта);
        }

        // GET: Паспорта/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Паспорта");

            if (id == null || _context.Паспорта == null)
            {
                return NotFound();
            }

            var паспорта = await _context.Паспорта.FindAsync(id);
            if (паспорта == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", паспорта.ID_призывника);
            return View(паспорта);
        }

        // POST: Паспорта/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_паспорта,Серия,Номер,Дата_выдачи,Кем_выдан,Место_рождения,ID_призывника,Адрес_прописки")] Паспорта паспорта)
        {
            if (id != паспорта.ID_паспорта)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(паспорта);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПаспортаExists(паспорта.ID_паспорта))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", паспорта.ID_призывника);
            return View(паспорта);
        }

        // GET: Паспорта/Delete/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Паспорта");

            if (id == null || _context.Паспорта == null)
            {
                return NotFound();
            }

            var паспорта = await _context.Паспорта
                .Include(п => п.Призывник)
                .FirstOrDefaultAsync(m => m.ID_паспорта == id);
            if (паспорта == null)
            {
                return NotFound();
            }

            return View(паспорта);
        }

        // POST: Паспорта/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Паспорта == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Паспорта'  is null.");
            }
            var паспорта = await _context.Паспорта.FindAsync(id);
            if (паспорта != null)
            {
                _context.Паспорта.Remove(паспорта);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПаспортаExists(int id)
        {
          return (_context.Паспорта?.Any(e => e.ID_паспорта == id)).GetValueOrDefault();
        }
    }
}
