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
    public class ТипыОбразовательныхОрганизацийController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ТипыОбразовательныхОрганизацийController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: ТипыОбразовательныхОрганизаций
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ТипыОбразовательныхОрганизаций");

            return _context.ТипыОбразовательныхОрганизаций != null ? 
                          View(await _context.ТипыОбразовательныхОрганизаций.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ТипыОбразовательныхОрганизаций'  is null.");
        }

        // GET: ТипыОбразовательныхОрганизаций/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "ТипыОбразовательныхОрганизаций");

            if (id == null || _context.ТипыОбразовательныхОрганизаций == null)
            {
                return NotFound();
            }

            var типыОбразовательныхОрганизаций = await _context.ТипыОбразовательныхОрганизаций
                .FirstOrDefaultAsync(m => m.ID_типа_образовательной_организации == id);
            if (типыОбразовательныхОрганизаций == null)
            {
                return NotFound();
            }

            return View(типыОбразовательныхОрганизаций);
        }

        // GET: ТипыОбразовательныхОрганизаций/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ТипыОбразовательныхОрганизаций");

            return View();
        }

        // POST: ТипыОбразовательныхОрганизаций/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_типа_образовательной_организации,Наименование_типа_образовательной_организации")] ТипыОбразовательныхОрганизаций типыОбразовательныхОрганизаций)
        {
            if (ModelState.IsValid)
            {
                _context.Add(типыОбразовательныхОрганизаций);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(типыОбразовательныхОрганизаций);
        }

        // GET: ТипыОбразовательныхОрганизаций/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ТипыОбразовательныхОрганизаций");

            if (id == null || _context.ТипыОбразовательныхОрганизаций == null)
            {
                return NotFound();
            }

            var типыОбразовательныхОрганизаций = await _context.ТипыОбразовательныхОрганизаций.FindAsync(id);
            if (типыОбразовательныхОрганизаций == null)
            {
                return NotFound();
            }
            return View(типыОбразовательныхОрганизаций);
        }

        // POST: ТипыОбразовательныхОрганизаций/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_типа_образовательной_организации,Наименование_типа_образовательной_организации")] ТипыОбразовательныхОрганизаций типыОбразовательныхОрганизаций)
        {
            if (id != типыОбразовательныхОрганизаций.ID_типа_образовательной_организации)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(типыОбразовательныхОрганизаций);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ТипыОбразовательныхОрганизацийExists(типыОбразовательныхОрганизаций.ID_типа_образовательной_организации))
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
            return View(типыОбразовательныхОрганизаций);
        }

        // GET: ТипыОбразовательныхОрганизаций/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ТипыОбразовательныхОрганизаций");

            if (id == null || _context.ТипыОбразовательныхОрганизаций == null)
            {
                return NotFound();
            }

            var типыОбразовательныхОрганизаций = await _context.ТипыОбразовательныхОрганизаций
                .FirstOrDefaultAsync(m => m.ID_типа_образовательной_организации == id);
            if (типыОбразовательныхОрганизаций == null)
            {
                return NotFound();
            }

            return View(типыОбразовательныхОрганизаций);
        }

        // POST: ТипыОбразовательныхОрганизаций/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ТипыОбразовательныхОрганизаций == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ТипыОбразовательныхОрганизаций'  is null.");
            }
            var типыОбразовательныхОрганизаций = await _context.ТипыОбразовательныхОрганизаций.FindAsync(id);
            if (типыОбразовательныхОрганизаций != null)
            {
                _context.ТипыОбразовательныхОрганизаций.Remove(типыОбразовательныхОрганизаций);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ТипыОбразовательныхОрганизацийExists(int id)
        {
          return (_context.ТипыОбразовательныхОрганизаций?.Any(e => e.ID_типа_образовательной_организации == id)).GetValueOrDefault();
        }
    }
}
