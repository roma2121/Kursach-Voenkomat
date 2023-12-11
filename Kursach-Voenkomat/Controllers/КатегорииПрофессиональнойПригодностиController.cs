using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kursach_Voenkomat
{
    public class КатегорииПрофессиональнойПригодностиController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public КатегорииПрофессиональнойПригодностиController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: КатегорииПрофессиональнойПригодности
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "КатегорииПрофессиональнойПригодности");

            return _context.КатегорииПрофессиональнойПригодности != null ? 
                          View(await _context.КатегорииПрофессиональнойПригодности.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.КатегорииПрофессиональнойПригодности'  is null.");
        }

        // GET: КатегорииПрофессиональнойПригодности/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "КатегорииПрофессиональнойПригодности");

            if (id == null || _context.КатегорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            var категорииПрофессиональнойПригодности = await _context.КатегорииПрофессиональнойПригодности
                .FirstOrDefaultAsync(m => m.ID_категории_профессиональной_пригодности == id);
            if (категорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            return View(категорииПрофессиональнойПригодности);
        }

        // GET: КатегорииПрофессиональнойПригодности/Create
        [Authorize(Roles = "MO, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "ISERT", "КатегорииПрофессиональнойПригодности");

            return View();
        }

        // POST: КатегорииПрофессиональнойПригодности/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_категории_профессиональной_пригодности,Категории_профессиональной_пригодности")] КатегорииПрофессиональнойПригодности категорииПрофессиональнойПригодности)
        {
            if (ModelState.IsValid)
            {
                _context.Add(категорииПрофессиональнойПригодности);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(категорииПрофессиональнойПригодности);
        }

        // GET: КатегорииПрофессиональнойПригодности/Edit/5
        [Authorize(Roles = "MO, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "КатегорииПрофессиональнойПригодности");

            if (id == null || _context.КатегорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            var категорииПрофессиональнойПригодности = await _context.КатегорииПрофессиональнойПригодности.FindAsync(id);
            if (категорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }
            return View(категорииПрофессиональнойПригодности);
        }

        // POST: КатегорииПрофессиональнойПригодности/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_категории_профессиональной_пригодности,Категории_профессиональной_пригодности")] КатегорииПрофессиональнойПригодности категорииПрофессиональнойПригодности)
        {
            if (id != категорииПрофессиональнойПригодности.ID_категории_профессиональной_пригодности)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(категорииПрофессиональнойПригодности);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!КатегорииПрофессиональнойПригодностиExists(категорииПрофессиональнойПригодности.ID_категории_профессиональной_пригодности))
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
            return View(категорииПрофессиональнойПригодности);
        }

        // GET: КатегорииПрофессиональнойПригодности/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "КатегорииПрофессиональнойПригодности");

            if (id == null || _context.КатегорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            var категорииПрофессиональнойПригодности = await _context.КатегорииПрофессиональнойПригодности
                .FirstOrDefaultAsync(m => m.ID_категории_профессиональной_пригодности == id);
            if (категорииПрофессиональнойПригодности == null)
            {
                return NotFound();
            }

            return View(категорииПрофессиональнойПригодности);
        }

        // POST: КатегорииПрофессиональнойПригодности/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.КатегорииПрофессиональнойПригодности == null)
            {
                return Problem("Entity set 'ApplicationDbContext.КатегорииПрофессиональнойПригодности'  is null.");
            }
            var категорииПрофессиональнойПригодности = await _context.КатегорииПрофессиональнойПригодности.FindAsync(id);
            if (категорииПрофессиональнойПригодности != null)
            {
                _context.КатегорииПрофессиональнойПригодности.Remove(категорииПрофессиональнойПригодности);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool КатегорииПрофессиональнойПригодностиExists(int id)
        {
          return (_context.КатегорииПрофессиональнойПригодности?.Any(e => e.ID_категории_профессиональной_пригодности == id)).GetValueOrDefault();
        }
    }
}
