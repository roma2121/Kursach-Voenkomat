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
    public class ПризывникРаботаController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ПризывникРаботаController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: ПризывникРабота
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ПризывникРабота");

            var applicationDbContext = _context.ПризывникРабота.Include(п => п.Призывник).Include(п => п.Работа);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ПризывникРабота/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "ПризывникРабота");

            if (id == null || _context.ПризывникРабота == null)
            {
                return NotFound();
            }

            var призывникРабота = await _context.ПризывникРабота
                .Include(п => п.Призывник)
                .Include(п => п.Работа)
                .FirstOrDefaultAsync(m => m.ID_призывник_работа == id);
            if (призывникРабота == null)
            {
                return NotFound();
            }

            return View(призывникРабота);
        }

        // GET: ПризывникРабота/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ПризывникРабота");

            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            ViewData["ID_работы"] = new SelectList(_context.Работа, "ID_работы", "Место_работы");
            return View();
        }

        // POST: ПризывникРабота/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_призывник_работа,ID_призывника,ID_работы")] ПризывникРабота призывникРабота)
        {
            if (ModelState.IsValid)
            {
                _context.Add(призывникРабота);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", призывникРабота.ID_призывника);
            ViewData["ID_работы"] = new SelectList(_context.Работа, "ID_работы", "ID_работы", призывникРабота.ID_работы);
            return View(призывникРабота);
        }

        // GET: ПризывникРабота/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ПризывникРабота");

            if (id == null || _context.ПризывникРабота == null)
            {
                return NotFound();
            }

            var призывникРабота = await _context.ПризывникРабота.FindAsync(id);
            if (призывникРабота == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", призывникРабота.ID_призывника);
            ViewData["ID_работы"] = new SelectList(_context.Работа, "ID_работы", "Место_работы", призывникРабота.ID_работы);
            return View(призывникРабота);
        }

        // POST: ПризывникРабота/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_призывник_работа,ID_призывника,ID_работы")] ПризывникРабота призывникРабота)
        {
            if (id != призывникРабота.ID_призывник_работа)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(призывникРабота);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПризывникРаботаExists(призывникРабота.ID_призывник_работа))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", призывникРабота.ID_призывника);
            ViewData["ID_работы"] = new SelectList(_context.Работа, "ID_работы", "ID_работы", призывникРабота.ID_работы);
            return View(призывникРабота);
        }

        // GET: ПризывникРабота/Delete/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ПризывникРабота");

            if (id == null || _context.ПризывникРабота == null)
            {
                return NotFound();
            }

            var призывникРабота = await _context.ПризывникРабота
                .Include(п => п.Призывник)
                .Include(п => п.Работа)
                .FirstOrDefaultAsync(m => m.ID_призывник_работа == id);
            if (призывникРабота == null)
            {
                return NotFound();
            }

            return View(призывникРабота);
        }

        // POST: ПризывникРабота/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ПризывникРабота == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ПризывникРабота'  is null.");
            }
            var призывникРабота = await _context.ПризывникРабота.FindAsync(id);
            if (призывникРабота != null)
            {
                _context.ПризывникРабота.Remove(призывникРабота);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПризывникРаботаExists(int id)
        {
          return (_context.ПризывникРабота?.Any(e => e.ID_призывник_работа == id)).GetValueOrDefault();
        }
    }
}
