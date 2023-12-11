using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kursach_Voenkomat
{
    public class ВоенноПрофессиональныеНаправленностиController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ВоенноПрофессиональныеНаправленностиController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: ВоенноПрофессиональныеНаправленности
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker,Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ВоенноПрофессиональныеНаправленности");

            return _context.ВоенноПрофессиональныеНаправленности != null ? 
                          View(await _context.ВоенноПрофессиональныеНаправленности.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ВоенноПрофессиональныеНаправленности'  is null.");
        }

        // GET: ВоенноПрофессиональныеНаправленности/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "ВоенноПрофессиональныеНаправленности");

            if (id == null || _context.ВоенноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            var военноПрофессиональныеНаправленности = await _context.ВоенноПрофессиональныеНаправленности
                .FirstOrDefaultAsync(m => m.ID_военно_профессиональной_направленности == id);
            if (военноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            return View(военноПрофессиональныеНаправленности);
        }

        // GET: ВоенноПрофессиональныеНаправленности/Create
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ВоенноПрофессиональныеНаправленности");

            return View();
        }

        // POST: ВоенноПрофессиональныеНаправленности/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_военно_профессиональной_направленности,Наименование_военно_профессиональной_направленности")] ВоенноПрофессиональныеНаправленности военноПрофессиональныеНаправленности)
        {
            if (ModelState.IsValid)
            {
                _context.Add(военноПрофессиональныеНаправленности);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(военноПрофессиональныеНаправленности);
        }

        // GET: ВоенноПрофессиональныеНаправленности/Edit/5
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ВоенноПрофессиональныеНаправленности");

            if (id == null || _context.ВоенноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            var военноПрофессиональныеНаправленности = await _context.ВоенноПрофессиональныеНаправленности.FindAsync(id);
            if (военноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }
            return View(военноПрофессиональныеНаправленности);
        }

        // POST: ВоенноПрофессиональныеНаправленности/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_военно_профессиональной_направленности,Наименование_военно_профессиональной_направленности")] ВоенноПрофессиональныеНаправленности военноПрофессиональныеНаправленности)
        {
            if (id != военноПрофессиональныеНаправленности.ID_военно_профессиональной_направленности)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(военноПрофессиональныеНаправленности);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВоенноПрофессиональныеНаправленностиExists(военноПрофессиональныеНаправленности.ID_военно_профессиональной_направленности))
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
            return View(военноПрофессиональныеНаправленности);
        }

        // GET: ВоенноПрофессиональныеНаправленности/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ВоенноПрофессиональныеНаправленности");

            if (id == null || _context.ВоенноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            var военноПрофессиональныеНаправленности = await _context.ВоенноПрофессиональныеНаправленности
                .FirstOrDefaultAsync(m => m.ID_военно_профессиональной_направленности == id);
            if (военноПрофессиональныеНаправленности == null)
            {
                return NotFound();
            }

            return View(военноПрофессиональныеНаправленности);
        }

        // POST: ВоенноПрофессиональныеНаправленности/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ВоенноПрофессиональныеНаправленности == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ВоенноПрофессиональныеНаправленности'  is null.");
            }
            var военноПрофессиональныеНаправленности = await _context.ВоенноПрофессиональныеНаправленности.FindAsync(id);
            if (военноПрофессиональныеНаправленности != null)
            {
                _context.ВоенноПрофессиональныеНаправленности.Remove(военноПрофессиональныеНаправленности);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВоенноПрофессиональныеНаправленностиExists(int id)
        {
          return (_context.ВоенноПрофессиональныеНаправленности?.Any(e => e.ID_военно_профессиональной_направленности == id)).GetValueOrDefault();
        }
    }
}
