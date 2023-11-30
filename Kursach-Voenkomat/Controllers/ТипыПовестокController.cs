using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kursach_Voenkomat
{
    public class ТипыПовестокController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ТипыПовестокController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: ТипыПовесток
        [Authorize(Roles = "voenkomat_worker, MO, State_services, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ТипыПовесток");

            return _context.ТипыПовесток != null ? 
                          View(await _context.ТипыПовесток.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ТипыПовесток'  is null.");
        }

        // GET: ТипыПовесток/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, State_services, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "ТипыПовесток");

            if (id == null || _context.ТипыПовесток == null)
            {
                return NotFound();
            }

            var типыПовесток = await _context.ТипыПовесток
                .FirstOrDefaultAsync(m => m.ID_типа_повестки == id);
            if (типыПовесток == null)
            {
                return NotFound();
            }

            return View(типыПовесток);
        }

        // GET: ТипыПовесток/Create
        [Authorize(Roles = "MO, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ТипыПовесток");

            return View();
        }

        // POST: ТипыПовесток/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_типа_повестки,Тип_повестки")] ТипыПовесток типыПовесток)
        {
            if (ModelState.IsValid)
            {
                _context.Add(типыПовесток);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(типыПовесток);
        }

        // GET: ТипыПовесток/Edit/5
        [Authorize(Roles = "MO, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ТипыПовесток");

            if (id == null || _context.ТипыПовесток == null)
            {
                return NotFound();
            }

            var типыПовесток = await _context.ТипыПовесток.FindAsync(id);
            if (типыПовесток == null)
            {
                return NotFound();
            }
            return View(типыПовесток);
        }

        // POST: ТипыПовесток/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_типа_повестки,Тип_повестки")] ТипыПовесток типыПовесток)
        {
            if (id != типыПовесток.ID_типа_повестки)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(типыПовесток);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ТипыПовестокExists(типыПовесток.ID_типа_повестки))
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
            return View(типыПовесток);
        }

        // GET: ТипыПовесток/Delete/5
        [Authorize(Roles = "MO, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ТипыПовесток");

            if (id == null || _context.ТипыПовесток == null)
            {
                return NotFound();
            }

            var типыПовесток = await _context.ТипыПовесток
                .FirstOrDefaultAsync(m => m.ID_типа_повестки == id);
            if (типыПовесток == null)
            {
                return NotFound();
            }

            return View(типыПовесток);
        }

        // POST: ТипыПовесток/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ТипыПовесток == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ТипыПовесток'  is null.");
            }
            var типыПовесток = await _context.ТипыПовесток.FindAsync(id);
            if (типыПовесток != null)
            {
                _context.ТипыПовесток.Remove(типыПовесток);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ТипыПовестокExists(int id)
        {
          return (_context.ТипыПовесток?.Any(e => e.ID_типа_повестки == id)).GetValueOrDefault();
        }
    }
}