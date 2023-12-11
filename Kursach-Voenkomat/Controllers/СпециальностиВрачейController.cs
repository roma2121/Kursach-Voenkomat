using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kursach_Voenkomat
{
    public class СпециальностиВрачейController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public СпециальностиВрачейController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: СпециальностиВрачей
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "СпециальностиВрачей");

            return _context.СпециальностиВрачей != null ? 
                          View(await _context.СпециальностиВрачей.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.СпециальностиВрачей'  is null.");
        }

        // GET: СпециальностиВрачей/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "СпециальностиВрачей");

            if (id == null || _context.СпециальностиВрачей == null)
            {
                return NotFound();
            }

            var специальностиВрачей = await _context.СпециальностиВрачей
                .FirstOrDefaultAsync(m => m.ID_специальности == id);
            if (специальностиВрачей == null)
            {
                return NotFound();
            }

            return View(специальностиВрачей);
        }

        // GET: СпециальностиВрачей/Create
        [Authorize(Roles = "Medical_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "СпециальностиВрачей");

            return View();
        }

        // POST: СпециальностиВрачей/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_специальности,Наименование_специальности")] СпециальностиВрачей специальностиВрачей)
        {
            if (ModelState.IsValid)
            {
                _context.Add(специальностиВрачей);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(специальностиВрачей);
        }

        // GET: СпециальностиВрачей/Edit/5
        [Authorize(Roles = "Medical_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "СпециальностиВрачей");

            if (id == null || _context.СпециальностиВрачей == null)
            {
                return NotFound();
            }

            var специальностиВрачей = await _context.СпециальностиВрачей.FindAsync(id);
            if (специальностиВрачей == null)
            {
                return NotFound();
            }
            return View(специальностиВрачей);
        }

        // POST: СпециальностиВрачей/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_специальности,Наименование_специальности")] СпециальностиВрачей специальностиВрачей)
        {
            if (id != специальностиВрачей.ID_специальности)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(специальностиВрачей);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!СпециальностиВрачейExists(специальностиВрачей.ID_специальности))
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
            return View(специальностиВрачей);
        }

        // GET: СпециальностиВрачей/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "СпециальностиВрачей");

            if (id == null || _context.СпециальностиВрачей == null)
            {
                return NotFound();
            }

            var специальностиВрачей = await _context.СпециальностиВрачей
                .FirstOrDefaultAsync(m => m.ID_специальности == id);
            if (специальностиВрачей == null)
            {
                return NotFound();
            }

            return View(специальностиВрачей);
        }

        // POST: СпециальностиВрачей/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.СпециальностиВрачей == null)
            {
                return Problem("Entity set 'ApplicationDbContext.СпециальностиВрачей'  is null.");
            }
            var специальностиВрачей = await _context.СпециальностиВрачей.FindAsync(id);
            if (специальностиВрачей != null)
            {
                _context.СпециальностиВрачей.Remove(специальностиВрачей);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool СпециальностиВрачейExists(int id)
        {
          return (_context.СпециальностиВрачей?.Any(e => e.ID_специальности == id)).GetValueOrDefault();
        }
    }
}
