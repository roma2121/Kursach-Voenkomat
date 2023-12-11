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
    public class ВрачиController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ВрачиController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: Врачи
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Врачи");

            var applicationDbContext = _context.Врачи.Include(в => в.Специальность);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Врачи/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "Врачи");

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
        [Authorize(Roles = "Medical_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Врачи");

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
        [Authorize(Roles = "Medical_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Врачи");

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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Врачи");

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
