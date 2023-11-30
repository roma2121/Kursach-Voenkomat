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
    public class УровниОбразованияController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public УровниОбразованияController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: УровниОбразования
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "УровниОбразования");

            return _context.УровниОбразования != null ? 
                          View(await _context.УровниОбразования.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.УровниОбразования'  is null.");
        }

        // GET: УровниОбразования/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "УровниОбразования");

            if (id == null || _context.УровниОбразования == null)
            {
                return NotFound();
            }

            var уровниОбразования = await _context.УровниОбразования
                .FirstOrDefaultAsync(m => m.ID_уровня_образования == id);
            if (уровниОбразования == null)
            {
                return NotFound();
            }

            return View(уровниОбразования);
        }

        // GET: УровниОбразования/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "УровниОбразования");

            return View();
        }

        // POST: УровниОбразования/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_уровня_образования,Наименование_уровня_образования")] УровниОбразования уровниОбразования)
        {
            if (ModelState.IsValid)
            {
                _context.Add(уровниОбразования);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(уровниОбразования);
        }

        // GET: УровниОбразования/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "УровниОбразования");

            if (id == null || _context.УровниОбразования == null)
            {
                return NotFound();
            }

            var уровниОбразования = await _context.УровниОбразования.FindAsync(id);
            if (уровниОбразования == null)
            {
                return NotFound();
            }
            return View(уровниОбразования);
        }

        // POST: УровниОбразования/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_уровня_образования,Наименование_уровня_образования")] УровниОбразования уровниОбразования)
        {
            if (id != уровниОбразования.ID_уровня_образования)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(уровниОбразования);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!УровниОбразованияExists(уровниОбразования.ID_уровня_образования))
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
            return View(уровниОбразования);
        }

        // GET: УровниОбразования/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "УровниОбразования");

            if (id == null || _context.УровниОбразования == null)
            {
                return NotFound();
            }

            var уровниОбразования = await _context.УровниОбразования
                .FirstOrDefaultAsync(m => m.ID_уровня_образования == id);
            if (уровниОбразования == null)
            {
                return NotFound();
            }

            return View(уровниОбразования);
        }

        // POST: УровниОбразования/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.УровниОбразования == null)
            {
                return Problem("Entity set 'ApplicationDbContext.УровниОбразования'  is null.");
            }
            var уровниОбразования = await _context.УровниОбразования.FindAsync(id);
            if (уровниОбразования != null)
            {
                _context.УровниОбразования.Remove(уровниОбразования);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool УровниОбразованияExists(int id)
        {
          return (_context.УровниОбразования?.Any(e => e.ID_уровня_образования == id)).GetValueOrDefault();
        }
    }
}
