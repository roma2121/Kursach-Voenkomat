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
    public class КатегорииГодностиController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public КатегорииГодностиController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: КатегорииГодности
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "КатегорииГодности");

            return _context.КатегорииГодности != null ? 
                          View(await _context.КатегорииГодности.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.КатегорииГодности'  is null.");
        }

        // GET: КатегорииГодности/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "КатегорииГодности");

            if (id == null || _context.КатегорииГодности == null)
            {
                return NotFound();
            }

            var категорииГодности = await _context.КатегорииГодности
                .FirstOrDefaultAsync(m => m.ID_категории_годности == id);
            if (категорииГодности == null)
            {
                return NotFound();
            }

            return View(категорииГодности);
        }

        // GET: КатегорииГодности/Create
        [Authorize(Roles = "MO, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "ISERT", "КатегорииГодности");

            return View();
        }

        // POST: КатегорииГодности/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_категории_годности,Наименование_категории")] КатегорииГодности категорииГодности)
        {
            if (ModelState.IsValid)
            {
                _context.Add(категорииГодности);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(категорииГодности);
        }

        // GET: КатегорииГодности/Edit/5
        [Authorize(Roles = "MO, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "КатегорииГодности");

            if (id == null || _context.КатегорииГодности == null)
            {
                return NotFound();
            }

            var категорииГодности = await _context.КатегорииГодности.FindAsync(id);
            if (категорииГодности == null)
            {
                return NotFound();
            }
            return View(категорииГодности);
        }

        // POST: КатегорииГодности/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_категории_годности,Наименование_категории")] КатегорииГодности категорииГодности)
        {
            if (id != категорииГодности.ID_категории_годности)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(категорииГодности);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!КатегорииГодностиExists(категорииГодности.ID_категории_годности))
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
            return View(категорииГодности);
        }

        // GET: КатегорииГодности/Delete/5
        [Authorize(Roles = "MO, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "КатегорииГодности");

            if (id == null || _context.КатегорииГодности == null)
            {
                return NotFound();
            }

            var категорииГодности = await _context.КатегорииГодности
                .FirstOrDefaultAsync(m => m.ID_категории_годности == id);
            if (категорииГодности == null)
            {
                return NotFound();
            }

            return View(категорииГодности);
        }

        // POST: КатегорииГодности/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.КатегорииГодности == null)
            {
                return Problem("Entity set 'ApplicationDbContext.КатегорииГодности'  is null.");
            }
            var категорииГодности = await _context.КатегорииГодности.FindAsync(id);
            if (категорииГодности != null)
            {
                _context.КатегорииГодности.Remove(категорииГодности);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool КатегорииГодностиExists(int id)
        {
          return (_context.КатегорииГодности?.Any(e => e.ID_категории_годности == id)).GetValueOrDefault();
        }
    }
}
