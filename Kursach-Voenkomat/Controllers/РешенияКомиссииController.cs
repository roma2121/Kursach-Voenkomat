using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kursach_Voenkomat
{
    public class РешенияКомиссииController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public РешенияКомиссииController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: РешенияКомиссии
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "РешенияКомиссии");

            var applicationDbContext = _context.РешенияКомиссии.Include(р => р.Призывник);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: РешенияКомиссии/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "РешенияКомиссии");

            if (id == null || _context.РешенияКомиссии == null)
            {
                return NotFound();
            }

            var решенияКомиссии = await _context.РешенияКомиссии
                .Include(р => р.Призывник)
                .FirstOrDefaultAsync(m => m.ID_решения == id);
            if (решенияКомиссии == null)
            {
                return NotFound();
            }

            return View(решенияКомиссии);
        }

        // GET: РешенияКомиссии/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "РешенияКомиссии");

            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            return View();
        }

        // POST: РешенияКомиссии/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_решения,Дата_вынесения_решения,Решение,ID_призывника")] РешенияКомиссии решенияКомиссии)
        {
            if (ModelState.IsValid)
            {
                _context.Add(решенияКомиссии);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", решенияКомиссии.ID_призывника);
            return View(решенияКомиссии);
        }

        // GET: РешенияКомиссии/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "РешенияКомиссии");

            if (id == null)
            {
                return NotFound();
            }

            var решенияКомиссии = await _context.РешенияКомиссии.FindAsync(id);
            if (решенияКомиссии == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", решенияКомиссии.ID_призывника);
            return View(решенияКомиссии);
        }

        // POST: РешенияКомиссии/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_решения,Дата_вынесения_решения,Решение,ID_призывника")] РешенияКомиссии решенияКомиссии)
        {
            if (id != решенияКомиссии.ID_решения)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(решенияКомиссии);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!РешенияКомиссииExists(решенияКомиссии.ID_решения))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", решенияКомиссии.ID_призывника);
            return View(решенияКомиссии);
        }


        // GET: РешенияКомиссии/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "РешенияКомиссии");

            if (id == null || _context.РешенияКомиссии == null)
            {
                return NotFound();
            }

            var решенияКомиссии = await _context.РешенияКомиссии
                .Include(р => р.Призывник)
                .FirstOrDefaultAsync(m => m.ID_решения == id);
            if (решенияКомиссии == null)
            {
                return NotFound();
            }

            return View(решенияКомиссии);
        }

        // POST: РешенияКомиссии/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.РешенияКомиссии == null)
            {
                return Problem("Entity set 'ApplicationDbContext.РешенияКомиссии'  is null.");
            }
            var решенияКомиссии = await _context.РешенияКомиссии.FindAsync(id);
            if (решенияКомиссии != null)
            {
                _context.РешенияКомиссии.Remove(решенияКомиссии);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool РешенияКомиссииExists(int id)
        {
          return (_context.РешенияКомиссии?.Any(e => e.ID_решения == id)).GetValueOrDefault();
        }
    }
}
