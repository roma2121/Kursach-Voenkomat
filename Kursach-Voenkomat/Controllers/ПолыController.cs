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
    public class ПолыController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ПолыController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: Полы
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Полы");

            return _context.Полы != null ? 
                          View(await _context.Полы.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Полы'  is null.");
        }

        // GET: Полы/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "Полы");

            if (id == null || _context.Полы == null)
            {
                return NotFound();
            }

            var полы = await _context.Полы
                .FirstOrDefaultAsync(m => m.ID_пола == id);
            if (полы == null)
            {
                return NotFound();
            }

            return View(полы);
        }

        // GET: Полы/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Полы");

            return View();
        }

        // POST: Полы/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_пола,Пол")] Полы полы)
        {
            if (ModelState.IsValid)
            {
                _context.Add(полы);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(полы);
        }

        // GET: Полы/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Полы");

            if (id == null || _context.Полы == null)
            {
                return NotFound();
            }

            var полы = await _context.Полы.FindAsync(id);
            if (полы == null)
            {
                return NotFound();
            }
            return View(полы);
        }

        // POST: Полы/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_пола,Пол")] Полы полы)
        {
            if (id != полы.ID_пола)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(полы);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПолыExists(полы.ID_пола))
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
            return View(полы);
        }

        // GET: Полы/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Полы");

            if (id == null || _context.Полы == null)
            {
                return NotFound();
            }

            var полы = await _context.Полы
                .FirstOrDefaultAsync(m => m.ID_пола == id);
            if (полы == null)
            {
                return NotFound();
            }

            return View(полы);
        }

        // POST: Полы/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Полы == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Полы'  is null.");
            }
            var полы = await _context.Полы.FindAsync(id);
            if (полы != null)
            {
                _context.Полы.Remove(полы);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПолыExists(int id)
        {
          return (_context.Полы?.Any(e => e.ID_пола == id)).GetValueOrDefault();
        }
    }
}
