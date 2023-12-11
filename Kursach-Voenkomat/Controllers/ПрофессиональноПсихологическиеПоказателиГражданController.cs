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
    public class ПрофессиональноПсихологическиеПоказателиГражданController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ПрофессиональноПсихологическиеПоказателиГражданController(ApplicationDbContext context, IAuditService auditService)
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

        // GET: ПрофессиональноПсихологическиеПоказателиГраждан
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ПрофессиональныеПоказателиГраждан");

            var applicationDbContext = _context.ПрофессиональноПсихологическиеПоказателиГраждан.Include(п => п.Призывник).Include(п => п.военноПрофессиональнаяНаправленность).Include(п => п.категорияПрофессиональнойПригодности);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ПрофессиональноПсихологическиеПоказателиГраждан/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "ПрофессиональныеПоказателиГраждан");

            if (id == null || _context.ПрофессиональноПсихологическиеПоказателиГраждан == null)
            {
                return NotFound();
            }

            var профессиональноПсихологическиеПоказателиГраждан = await _context.ПрофессиональноПсихологическиеПоказателиГраждан
                .Include(п => п.Призывник)
                .Include(п => п.военноПрофессиональнаяНаправленность)
                .Include(п => п.категорияПрофессиональнойПригодности)
                .FirstOrDefaultAsync(m => m.ID_ПП_показателей == id);
            if (профессиональноПсихологическиеПоказателиГраждан == null)
            {
                return NotFound();
            }

            return View(профессиональноПсихологическиеПоказателиГраждан);
        }

        // GET: ПрофессиональноПсихологическиеПоказателиГраждан/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ПрофессиональныеПоказателиГраждан");

            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            ViewData["ID_военно_профессиональной_направленности"] = new SelectList(_context.ВоенноПрофессиональныеНаправленности, "ID_военно_профессиональной_направленности", "Наименование_военно_профессиональной_направленности");
            ViewData["ID_категории_профессиональной_пригодности"] = new SelectList(_context.КатегорииПрофессиональнойПригодности, "ID_категории_профессиональной_пригодности", "Категории_профессиональной_пригодности");
            return View();
        }

        // POST: ПрофессиональноПсихологическиеПоказателиГраждан/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_ПП_показателей,Познавательные_способности,Адаптационные_способности,ID_категории_профессиональной_пригодности,ID_военно_профессиональной_направленности,ID_призывника")] ПрофессиональноПсихологическиеПоказателиГраждан профессиональноПсихологическиеПоказателиГраждан)
        {
            if (ModelState.IsValid)
            {
                _context.Add(профессиональноПсихологическиеПоказателиГраждан);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", профессиональноПсихологическиеПоказателиГраждан.ID_призывника);
            ViewData["ID_военно_профессиональной_направленности"] = new SelectList(_context.ВоенноПрофессиональныеНаправленности, "ID_военно_профессиональной_направленности", "ID_военно_профессиональной_направленности", профессиональноПсихологическиеПоказателиГраждан.ID_военно_профессиональной_направленности);
            ViewData["ID_категории_профессиональной_пригодности"] = new SelectList(_context.КатегорииПрофессиональнойПригодности, "ID_категории_профессиональной_пригодности", "ID_категории_профессиональной_пригодности", профессиональноПсихологическиеПоказателиГраждан.ID_категории_профессиональной_пригодности);
            return View(профессиональноПсихологическиеПоказателиГраждан);
        }

        // GET: ПрофессиональноПсихологическиеПоказателиГраждан/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ПрофессиональныеПоказателиГраждан");

            if (id == null || _context.ПрофессиональноПсихологическиеПоказателиГраждан == null)
            {
                return NotFound();
            }

            var профессиональноПсихологическиеПоказателиГраждан = await _context.ПрофессиональноПсихологическиеПоказателиГраждан.FindAsync(id);
            if (профессиональноПсихологическиеПоказателиГраждан == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", профессиональноПсихологическиеПоказателиГраждан.ID_призывника);
            ViewData["ID_военно_профессиональной_направленности"] = new SelectList(_context.ВоенноПрофессиональныеНаправленности, "ID_военно_профессиональной_направленности", "Наименование_военно_профессиональной_направленности", профессиональноПсихологическиеПоказателиГраждан.ID_военно_профессиональной_направленности);
            ViewData["ID_категории_профессиональной_пригодности"] = new SelectList(_context.КатегорииПрофессиональнойПригодности, "ID_категории_профессиональной_пригодности", "Категории_профессиональной_пригодности", профессиональноПсихологическиеПоказателиГраждан.ID_категории_профессиональной_пригодности);
            return View(профессиональноПсихологическиеПоказателиГраждан);
        }

        // POST: ПрофессиональноПсихологическиеПоказателиГраждан/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_ПП_показателей,Познавательные_способности,Адаптационные_способности,ID_категории_профессиональной_пригодности,ID_военно_профессиональной_направленности,ID_призывника")] ПрофессиональноПсихологическиеПоказателиГраждан профессиональноПсихологическиеПоказателиГраждан)
        {
            if (id != профессиональноПсихологическиеПоказателиГраждан.ID_ПП_показателей)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(профессиональноПсихологическиеПоказателиГраждан);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПрофессиональноПсихологическиеПоказателиГражданExists(профессиональноПсихологическиеПоказателиГраждан.ID_ПП_показателей))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", профессиональноПсихологическиеПоказателиГраждан.ID_призывника);
            ViewData["ID_военно_профессиональной_направленности"] = new SelectList(_context.ВоенноПрофессиональныеНаправленности, "ID_военно_профессиональной_направленности", "ID_военно_профессиональной_направленности", профессиональноПсихологическиеПоказателиГраждан.ID_военно_профессиональной_направленности);
            ViewData["ID_категории_профессиональной_пригодности"] = new SelectList(_context.КатегорииПрофессиональнойПригодности, "ID_категории_профессиональной_пригодности", "ID_категории_профессиональной_пригодности", профессиональноПсихологическиеПоказателиГраждан.ID_категории_профессиональной_пригодности);
            return View(профессиональноПсихологическиеПоказателиГраждан);
        }

        // GET: ПрофессиональноПсихологическиеПоказателиГраждан/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ПрофессиональныеПоказателиГраждан");

            if (id == null || _context.ПрофессиональноПсихологическиеПоказателиГраждан == null)
            {
                return NotFound();
            }

            var профессиональноПсихологическиеПоказателиГраждан = await _context.ПрофессиональноПсихологическиеПоказателиГраждан
                .Include(п => п.Призывник)
                .Include(п => п.военноПрофессиональнаяНаправленность)
                .Include(п => п.категорияПрофессиональнойПригодности)
                .FirstOrDefaultAsync(m => m.ID_ПП_показателей == id);
            if (профессиональноПсихологическиеПоказателиГраждан == null)
            {
                return NotFound();
            }

            return View(профессиональноПсихологическиеПоказателиГраждан);
        }

        // POST: ПрофессиональноПсихологическиеПоказателиГраждан/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ПрофессиональноПсихологическиеПоказателиГраждан == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ПрофессиональноПсихологическиеПоказателиГраждан'  is null.");
            }
            var профессиональноПсихологическиеПоказателиГраждан = await _context.ПрофессиональноПсихологическиеПоказателиГраждан.FindAsync(id);
            if (профессиональноПсихологическиеПоказателиГраждан != null)
            {
                _context.ПрофессиональноПсихологическиеПоказателиГраждан.Remove(профессиональноПсихологическиеПоказателиГраждан);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПрофессиональноПсихологическиеПоказателиГражданExists(int id)
        {
          return (_context.ПрофессиональноПсихологическиеПоказателиГраждан?.Any(e => e.ID_ПП_показателей == id)).GetValueOrDefault();
        }
    }
}
