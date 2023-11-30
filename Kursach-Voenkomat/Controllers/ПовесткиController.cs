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
    public class ПовесткиController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ПовесткиController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: Повестки
        [Authorize(Roles = "voenkomat_worker, State_services, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Повестки");

            var applicationDbContext = _context.Повестки.Include(п => п.Призывник).Include(п => п.статусЯвки).Include(п => п.типПовестки);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Повестки/Details/5
        [Authorize(Roles = "voenkomat_worker, State_services, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Повестки == null)
            {
                return NotFound();
            }

            var повестки = await _context.Повестки
                .Include(п => п.Призывник)
                .Include(п => п.статусЯвки)
                .Include(п => п.типПовестки)
                .FirstOrDefaultAsync(m => m.ID_повестки == id);
            if (повестки == null)
            {
                return NotFound();
            }

            return View(повестки);
        }

        // GET: Повестки/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Повестки");

            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            ViewData["ID_наименования_статуса_явки"] = new SelectList(_context.ВидыСтатусовЯвки, "ID_наименования_статуса_явки", "Наименование_статуса_явки");
            ViewData["ID_типа_повестки"] = new SelectList(_context.ТипыПовесток, "ID_типа_повестки", "Тип_повестки");
            return View();
        }

        // POST: Повестки/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_повестки,ID_призывника,ID_типа_повестки,Дата_выписки,ID_наименования_статуса_явки,Дата_явки")] Повестки повестки)
        {
            if (ModelState.IsValid)
            {
                _context.Add(повестки);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", повестки.ID_призывника);
            ViewData["ID_наименования_статуса_явки"] = new SelectList(_context.ВидыСтатусовЯвки, "ID_наименования_статуса_явки", "ID_наименования_статуса_явки", повестки.ID_наименования_статуса_явки);
            ViewData["ID_типа_повестки"] = new SelectList(_context.ТипыПовесток, "ID_типа_повестки", "ID_типа_повестки", повестки.ID_типа_повестки);
            return View(повестки);
        }

        // GET: Повестки/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Повестки");

            if (id == null || _context.Повестки == null)
            {
                return NotFound();
            }

            var повестки = await _context.Повестки.FindAsync(id);
            if (повестки == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", повестки.ID_призывника);
            ViewData["ID_наименования_статуса_явки"] = new SelectList(_context.ВидыСтатусовЯвки, "ID_наименования_статуса_явки", "Наименование_статуса_явки", повестки.ID_наименования_статуса_явки);
            ViewData["ID_типа_повестки"] = new SelectList(_context.ТипыПовесток, "ID_типа_повестки", "Тип_повестки", повестки.ID_типа_повестки);
            return View(повестки);
        }

        // POST: Повестки/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_повестки,ID_призывника,ID_типа_повестки,Дата_выписки,ID_наименования_статуса_явки,Дата_явки")] Повестки повестки)
        {
            if (id != повестки.ID_повестки)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(повестки);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПовесткиExists(повестки.ID_повестки))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", повестки.ID_призывника);
            ViewData["ID_наименования_статуса_явки"] = new SelectList(_context.ВидыСтатусовЯвки, "ID_наименования_статуса_явки", "ID_наименования_статуса_явки", повестки.ID_наименования_статуса_явки);
            ViewData["ID_типа_повестки"] = new SelectList(_context.ТипыПовесток, "ID_типа_повестки", "ID_типа_повестки", повестки.ID_типа_повестки);
            return View(повестки);
        }

        // GET: Повестки/Delete/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Повестки");

            if (id == null || _context.Повестки == null)
            {
                return NotFound();
            }

            var повестки = await _context.Повестки
                .Include(п => п.Призывник)
                .Include(п => п.статусЯвки)
                .Include(п => п.типПовестки)
                .FirstOrDefaultAsync(m => m.ID_повестки == id);
            if (повестки == null)
            {
                return NotFound();
            }

            return View(повестки);
        }

        // POST: Повестки/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Повестки == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Повестки'  is null.");
            }
            var повестки = await _context.Повестки.FindAsync(id);
            if (повестки != null)
            {
                _context.Повестки.Remove(повестки);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПовесткиExists(int id)
        {
          return (_context.Повестки?.Any(e => e.ID_повестки == id)).GetValueOrDefault();
        }
    }
}
