using Kursach_Voenkomat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kursach_Voenkomat.Controllers
{
    public class RecordController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.RecordModel != null ?
                         View(await _context.RecordModel.ToListAsync()) :
                         Problem("Entity set 'ApplicationDbContext.RecordModel'  is null.");
        }

        [HttpGet]
        public async Task<IActionResult> FilterByDate(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.RecordModel
                    .Where(record => record.Дата_посещения >= startDate && record.Дата_посещения <= endDate)
                    .ToListAsync();

                return View(nameof(Index), filteredRecords);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ReportByDay(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.RecordModel
                    .Where(record => record.Дата_посещения >= startDate && record.Дата_посещения <= endDate)
                    .ToListAsync();

                // Группировка записей по дням
                var dailyCounts = filteredRecords
                    .GroupBy(record => new { record.Дата_посещения.Year, record.Дата_посещения.Month, record.Дата_посещения.Day })
                    .Select(group => new { Day = new DateTime(group.Key.Year, group.Key.Month, group.Key.Day), Count = group.Count() })
                    .OrderBy(item => item.Day)
                    .ToList();

                // Создание данных для графика
                var days = dailyCounts.Select(item => item.Day.ToString("yyyy-MM-dd"));
                var counts = dailyCounts.Select(item => item.Count);

                ViewBag.Days = days;
                ViewBag.Counts = counts;

                return View("~/Views/Reports/ReportByDay.cshtml"); // Представление для отображения графика
            }

            else
            {
                var allRecords = await _context.RecordModel.ToListAsync();

                var dailyCounts = allRecords
                    .GroupBy(record => new { record.Дата_посещения.Year, record.Дата_посещения.Month, record.Дата_посещения.Day })
                    .Select(group => new { Day = new DateTime(group.Key.Year, group.Key.Month, group.Key.Day), Count = group.Count() })
                    .OrderBy(item => item.Day)
                    .ToList();

                var days = dailyCounts.Select(item => item.Day.ToString("yyyy-MM-dd"));
                var counts = dailyCounts.Select(item => item.Count);

                ViewBag.Days = days;
                ViewBag.Counts = counts;

                return View("~/Views/Reports/ReportByDay.cshtml"); // Представление для отображения графика
            }
        }

    }
}
