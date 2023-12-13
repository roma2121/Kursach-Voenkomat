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

namespace Kursach_Voenkomat.Controllers
{
    public class ПриписныеController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ПриписныеController(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }


        [HttpGet]
        public async Task<IActionResult> FilterByDate(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.Приписные.Include(п => п.Призывник).Include(п => п.Решение)
                    .Where(record => record.Дата_выдачи >= startDate && record.Дата_выдачи <= endDate)
                    .ToListAsync();

                return View(nameof(Index), filteredRecords);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ReportByMonth(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.Приписные.Include(п => п.Призывник).Include(п => п.Решение)
                    .Where(record => record.Дата_выдачи >= startDate && record.Дата_выдачи <= endDate)
                    .ToListAsync();

                // Группировка записей по месяцам
                var monthlyCounts = filteredRecords
                    .GroupBy(record => new { record.Дата_выдачи.Year, record.Дата_выдачи.Month })
                    .Select(group => new { Month = new DateTime(group.Key.Year, group.Key.Month, 1), Count = group.Count() })
                    .OrderBy(item => item.Month)
                    .ToList();

                // Создание данных для графика
                var months = monthlyCounts.Select(item => item.Month.ToString("yyyy-MM"));
                var counts = monthlyCounts.Select(item => item.Count);

                ViewBag.Months = months;
                ViewBag.Counts = counts;

                return View("~/Views/Reports/ReportByMonth.cshtml"); // Представление для отображения графика
            }

            else
            {
                // Логика для формирования графика из всех записей, если даты фильтрации не указаны
                var allRecords = await _context.Приписные.Include(п => п.Призывник).Include(п => п.Решение).ToListAsync();

                var monthlyCounts = allRecords
                    .GroupBy(record => new { record.Дата_выдачи.Year, record.Дата_выдачи.Month })
                    .Select(group => new { Month = new DateTime(group.Key.Year, group.Key.Month, 1), Count = group.Count() })
                    .OrderBy(item => item.Month)
                    .ToList();

                var months = monthlyCounts.Select(item => item.Month.ToString("yyyy-MM"));
                var counts = monthlyCounts.Select(item => item.Count);

                ViewBag.Months = months;
                ViewBag.Counts = counts;

                return View("~/Views/Reports/ReportByMonth.cshtml"); // Представление для отображения графика
            }
        }

        // GET: Приписные
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Приписные");

            var applicationDbContext = _context.Приписные.Include(п => п.Призывник).Include(п => п.Решение);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Приписные/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "Приписные");

            if (id == null || _context.Приписные == null)
            {
                return NotFound();
            }

            var приписные = await _context.Приписные
                .Include(п => п.Призывник)
                .Include(п => п.Решение)
                .FirstOrDefaultAsync(m => m.ID_приписного == id);
            if (приписные == null)
            {
                return NotFound();
            }

            return View(приписные);
        }

        // GET: Приписные/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Приписные");

            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            ViewData["ID_решения"] = new SelectList(_context.РешенияКомиссии, "ID_решения", "Решение");
            return View();
        }

        // POST: Приписные/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_приписного,ID_призывника,ID_решения")] Приписные приписные)
        {
            if (ModelState.IsValid)
            {
                приписные.Дата_выдачи = DateTime.Now;
                _context.Add(приписные);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", приписные.ID_призывника);
            ViewData["ID_решения"] = new SelectList(_context.РешенияКомиссии, "ID_решения", "ID_решения", приписные.ID_решения);
            return View(приписные);
        }

        // GET: Приписные/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Приписные");

            if (id == null || _context.Приписные == null)
            {
                return NotFound();
            }

            var приписные = await _context.Приписные.FindAsync(id);
            if (приписные == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", приписные.ID_призывника);
            ViewData["ID_решения"] = new SelectList(_context.РешенияКомиссии, "ID_решения", "Решение", приписные.ID_решения);
            return View(приписные);
        }

        // POST: Приписные/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_приписного,ID_призывника,ID_решения")] Приписные приписные)
        {
            if (id != приписные.ID_приписного)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(приписные);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПриписныеExists(приписные.ID_приписного))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", приписные.ID_призывника);
            ViewData["ID_решения"] = new SelectList(_context.РешенияКомиссии, "ID_решения", "ID_решения", приписные.ID_решения);
            return View(приписные);
        }

        // GET: Приписные/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Приписные");

            if (id == null || _context.Приписные == null)
            {
                return NotFound();
            }

            var приписные = await _context.Приписные
                .Include(п => п.Призывник)
                .Include(п => п.Решение)
                .FirstOrDefaultAsync(m => m.ID_приписного == id);
            if (приписные == null)
            {
                return NotFound();
            }

            return View(приписные);
        }

        // POST: Приписные/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Приписные == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Приписные'  is null.");
            }
            var приписные = await _context.Приписные.FindAsync(id);
            if (приписные != null)
            {
                _context.Приписные.Remove(приписные);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПриписныеExists(int id)
        {
          return (_context.Приписные?.Any(e => e.ID_приписного == id)).GetValueOrDefault();
        }
    }
}
