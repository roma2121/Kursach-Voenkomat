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
    public class МедицинскиеДокументыController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public МедицинскиеДокументыController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: МедицинскиеДокументы
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "МедицинскиеДокументы");

            var applicationDbContext = _context.МедицинскиеДокументы.Include(м => м.Призывник);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: МедицинскиеДокументы/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "МедицинскиеДокументы");

            if (id == null || _context.МедицинскиеДокументы == null)
            {
                return NotFound();
            }

            var медицинскиеДокументы = await _context.МедицинскиеДокументы
                .Include(м => м.Призывник)
                .FirstOrDefaultAsync(m => m.ID_медицинского_документа == id);
            if (медицинскиеДокументы == null)
            {
                return NotFound();
            }

            return View(медицинскиеДокументы);
        }

        // GET: МедицинскиеДокументы/Create
        [Authorize(Roles = "Medical_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "МедицинскиеДокументы");

            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            return View();
        }

        // POST: МедицинскиеДокументы/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_медицинского_документа,ID_призывника,Диагноз,Наименование_медицинской_организации,Дата_выписки_медицинского_заключения")] МедицинскиеДокументы медицинскиеДокументы)
        {
            if (ModelState.IsValid)
            {
                _context.Add(медицинскиеДокументы);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", медицинскиеДокументы.ID_призывника);
            return View(медицинскиеДокументы);
        }

        // GET: МедицинскиеДокументы/Edit/5
        [Authorize(Roles = "Medical_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "МедицинскиеДокументы");

            if (id == null || _context.МедицинскиеДокументы == null)
            {
                return NotFound();
            }

            var медицинскиеДокументы = await _context.МедицинскиеДокументы.FindAsync(id);
            if (медицинскиеДокументы == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", медицинскиеДокументы.ID_призывника);
            return View(медицинскиеДокументы);
        }

        // POST: МедицинскиеДокументы/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_медицинского_документа,ID_призывника,Диагноз,Наименование_медицинской_организации,Дата_выписки_медицинского_заключения")] МедицинскиеДокументы медицинскиеДокументы)
        {
            if (id != медицинскиеДокументы.ID_медицинского_документа)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(медицинскиеДокументы);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!МедицинскиеДокументыExists(медицинскиеДокументы.ID_медицинского_документа))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", медицинскиеДокументы.ID_призывника);
            return View(медицинскиеДокументы);
        }

        // GET: МедицинскиеДокументы/Delete/5
        [Authorize(Roles = "Medical_worker, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "МедицинскиеДокументы");

            if (id == null || _context.МедицинскиеДокументы == null)
            {
                return NotFound();
            }

            var медицинскиеДокументы = await _context.МедицинскиеДокументы
                .Include(м => м.Призывник)
                .FirstOrDefaultAsync(m => m.ID_медицинского_документа == id);
            if (медицинскиеДокументы == null)
            {
                return NotFound();
            }

            return View(медицинскиеДокументы);
        }

        // POST: МедицинскиеДокументы/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.МедицинскиеДокументы == null)
            {
                return Problem("Entity set 'ApplicationDbContext.МедицинскиеДокументы'  is null.");
            }
            var медицинскиеДокументы = await _context.МедицинскиеДокументы.FindAsync(id);
            if (медицинскиеДокументы != null)
            {
                _context.МедицинскиеДокументы.Remove(медицинскиеДокументы);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool МедицинскиеДокументыExists(int id)
        {
          return (_context.МедицинскиеДокументы?.Any(e => e.ID_медицинского_документа == id)).GetValueOrDefault();
        }
    }
}
