using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kursach_Voenkomat
{
    public class ВидыСтатусовЯвкиController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ВидыСтатусовЯвкиController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: ВидыСтатусовЯвки
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ВидыСтатусовЯвки");

            return _context.ВидыСтатусовЯвки != null ? 
                          View(await _context.ВидыСтатусовЯвки.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ВидыСтатусовЯвки'  is null.");
        }

        // GET: ВидыСтатусовЯвки/Details/5
        [Authorize(Roles = "MO, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "ВидыСтатусовЯвки");

            if (id == null || _context.ВидыСтатусовЯвки == null)
            {
                return NotFound();
            }

            var видыСтатусовЯвки = await _context.ВидыСтатусовЯвки
                .FirstOrDefaultAsync(m => m.ID_наименования_статуса_явки == id);
            if (видыСтатусовЯвки == null)
            {
                return NotFound();
            }

            return View(видыСтатусовЯвки);
        }

        // GET: ВидыСтатусовЯвки/Create
        [Authorize(Roles = "MO, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ВидыСтатусовЯвки");

            return View();
        }

        // POST: ВидыСтатусовЯвки/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_наименования_статуса_явки,Наименование_статуса_явки")] ВидыСтатусовЯвки видыСтатусовЯвки)
        {
            if (ModelState.IsValid)
            {
                _context.Add(видыСтатусовЯвки);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(видыСтатусовЯвки);
        }

        // GET: ВидыСтатусовЯвки/Edit/5
        [Authorize(Roles = "MO, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ВидыСтатусовЯвки");

            if (id == null || _context.ВидыСтатусовЯвки == null)
            {
                return NotFound();
            }

            var видыСтатусовЯвки = await _context.ВидыСтатусовЯвки.FindAsync(id);
            if (видыСтатусовЯвки == null)
            {
                return NotFound();
            }
            return View(видыСтатусовЯвки);
        }

        // POST: ВидыСтатусовЯвки/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_наименования_статуса_явки,Наименование_статуса_явки")] ВидыСтатусовЯвки видыСтатусовЯвки)
        {
            if (id != видыСтатусовЯвки.ID_наименования_статуса_явки)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(видыСтатусовЯвки);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВидыСтатусовЯвкиExists(видыСтатусовЯвки.ID_наименования_статуса_явки))
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
            return View(видыСтатусовЯвки);
        }

        // GET: ВидыСтатусовЯвки/Delete/5
        [Authorize(Roles = "MO, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ВидыСтатусовЯвки");

            if (id == null || _context.ВидыСтатусовЯвки == null)
            {
                return NotFound();
            }

            var видыСтатусовЯвки = await _context.ВидыСтатусовЯвки
                .FirstOrDefaultAsync(m => m.ID_наименования_статуса_явки == id);
            if (видыСтатусовЯвки == null)
            {
                return NotFound();
            }

            return View(видыСтатусовЯвки);
        }

        // POST: ВидыСтатусовЯвки/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ВидыСтатусовЯвки == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ВидыСтатусовЯвки'  is null.");
            }
            var видыСтатусовЯвки = await _context.ВидыСтатусовЯвки.FindAsync(id);
            if (видыСтатусовЯвки != null)
            {
                _context.ВидыСтатусовЯвки.Remove(видыСтатусовЯвки);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВидыСтатусовЯвкиExists(int id)
        {
          return (_context.ВидыСтатусовЯвки?.Any(e => e.ID_наименования_статуса_явки == id)).GetValueOrDefault();
        }
    }
}