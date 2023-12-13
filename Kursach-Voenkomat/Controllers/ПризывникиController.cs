using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kursach_Voenkomat
{
    public class ПризывникиController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuditService _auditService;

        public ПризывникиController(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<IActionResult> ReportByMonth(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.Призывники.Include(п => п.Пол)
                    .Where(record => record.Дата_регистрации >= startDate && record.Дата_регистрации <= endDate)
                    .ToListAsync();

                // Группировка записей по месяцам
                var monthlyCounts = filteredRecords
                    .GroupBy(record => new { record.Дата_регистрации.Year, record.Дата_регистрации.Month })
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
                var allRecords = await _context.Призывники.Include(с => с.Пол).ToListAsync();

                var monthlyCounts = allRecords
                    .GroupBy(record => new { record.Дата_регистрации.Year, record.Дата_регистрации.Month })
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


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied"); // Отображение страницы с сообщением об отказе в доступе
        }

        // GET: Призывники
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, State_services, Administrator")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Призывники");

            var applicationDbContext = _context.Призывники.Include(п => п.Пол);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Призывники/Details/5
        [Authorize(Roles = "voenkomat_worker, MO, Medical_worker, State_services, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DETAILS", "Призывники");

            if (id == null || _context.Призывники == null)
            {
                return NotFound();
            }

            var призывники = await _context.Призывники
                .Include(п => п.Пол)
                .FirstOrDefaultAsync(m => m.ID_призывника == id);
            if (призывники == null)
            {
                return NotFound();
            }

            return View(призывники);
        }

        // GET: Призывники/Create
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public IActionResult Create(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Призывники");

            ViewData["ID_пола"] = new SelectList(_context.Полы, "ID_пола", "Пол");

            RecordModel record = _context.RecordModel.FirstOrDefault(p => p.ID_посещения == id);

            if (record != null)
            {
                Призывники призывник = new Призывники
                {
                    Фамилия = record.Фамилия,
                    Имя = record.Имя,
                    Отчество = record.Отчество,
                    Дата_рождения = record.Дата_рождения,
                };

                return View(призывник);
            }
            else
            {
                return View();
            }
        }

        // POST: Призывники/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Create([Bind("ID_призывника,Имя,Фамилия,Отчество,Дата_рождения,Номер_телефона,ID_пола,Дата_регистрации")] Призывники призывники)
        {
            if (ModelState.IsValid)
            {
                призывники.Дата_регистрации = DateTime.Now;
                _context.Add(призывники);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var полыList = _context.Полы.Select(п => new { ID_пола = п.ID_пола, Наименование = п.Пол }).ToList();
            ViewData["ID_пола"] = new SelectList(полыList, "ID_пола", "Наименование", призывники.ID_пола);

            return View(призывники);
        }

        // GET: Призывники/Edit/5
        [Authorize(Roles = "voenkomat_worker, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Призывники");

            if (id == null || _context.Призывники == null)
            {
                return NotFound();
            }

            var призывники = await _context.Призывники.FindAsync(id);
            if (призывники == null)
            {
                return NotFound();
            }

            var полыList = _context.Полы.Select(п => new { ID_пола = п.ID_пола, Наименование = п.Пол }).ToList();
            ViewData["ID_пола"] = new SelectList(полыList, "ID_пола", "Наименование", призывники.ID_пола);

            return View(призывники);
        }

        // POST: Призывники/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_призывника,Имя,Фамилия,Отчество,Дата_рождения,Номер_телефона,ID_пола,Дата_регистрации")] Призывники призывники)
        {
            if (id != призывники.ID_призывника)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(призывники);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПризывникиExists(призывники.ID_призывника))
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

            var полыList = _context.Полы.Select(п => new { ID_пола = п.ID_пола, Наименование = п.Пол }).ToList();
            ViewData["ID_пола"] = new SelectList(полыList, "ID_пола", "Наименование", призывники.ID_пола);

            return View(призывники);
        }

        // GET: Призывники/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Призывники");

            if (id == null || _context.Призывники == null)
            {
                return NotFound();
            }

            var призывники = await _context.Призывники
                .Include(п => п.Пол)
                .FirstOrDefaultAsync(m => m.ID_призывника == id);
            if (призывники == null)
            {
                return NotFound();
            }

            return View(призывники);
        }

        // POST: Призывники/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Призывники == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Призывники'  is null.");
            }
            var призывники = await _context.Призывники.FindAsync(id);
            if (призывники != null)
            {
                _context.Призывники.Remove(призывники);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПризывникиExists(int id)
        {
          return (_context.Призывники?.Any(e => e.ID_призывника == id)).GetValueOrDefault();
        }
    }
}
