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
    public class РаботаController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public РаботаController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: Работа
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Работа");

            return _context.Работа != null ? 
                          View(await _context.Работа.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Работа'  is null.");
        }

        // GET: Работа/Details/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "Работа");

            if (id == null || _context.Работа == null)
            {
                return NotFound();
            }

            var работа = await _context.Работа
                .FirstOrDefaultAsync(m => m.ID_работы == id);
            if (работа == null)
            {
                return NotFound();
            }

            return View(работа);
        }

        // GET: Работа/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Работа");

            return View();
        }

        // POST: Работа/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Место_работы,ID_работы")] Работа работа)
        {
            if (ModelState.IsValid)
            {
                _context.Add(работа);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(работа);
        }

        // GET: Работа/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Работа");

            if (id == null || _context.Работа == null)
            {
                return NotFound();
            }

            var работа = await _context.Работа.FindAsync(id);
            if (работа == null)
            {
                return NotFound();
            }
            return View(работа);
        }

        // POST: Работа/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Место_работы,ID_работы")] Работа работа)
        {
            if (id != работа.ID_работы)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(работа);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!РаботаExists(работа.ID_работы))
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
            return View(работа);
        }

        // GET: Работа/Delete/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Работа");

            if (id == null || _context.Работа == null)
            {
                return NotFound();
            }

            var работа = await _context.Работа
                .FirstOrDefaultAsync(m => m.ID_работы == id);
            if (работа == null)
            {
                return NotFound();
            }

            return View(работа);
        }

        // POST: Работа/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Работа == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Работа'  is null.");
            }
            var работа = await _context.Работа.FindAsync(id);
            if (работа != null)
            {
                _context.Работа.Remove(работа);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool РаботаExists(int id)
        {
          return (_context.Работа?.Any(e => e.ID_работы == id)).GetValueOrDefault();
        }
    }
}
