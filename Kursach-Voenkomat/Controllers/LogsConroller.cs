using Kursach_Voenkomat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kursach_Voenkomat.Controllers
{
    public class LogsController : Controller
    {
        private readonly UserContext _dbContext;
        private readonly ILogger<LogsController> _logger;

        public LogsController(UserContext dbContext, ILogger<LogsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            try
            {
                var auditLogs = _dbContext.Logs.ToList(); // Здесь может быть ваш запрос к аудит-логам

                // Логгирование информации о запросе логов
                _logger.LogInformation("Запрошены аудит-логи");

                return View(auditLogs);
            }
            catch (Exception ex)
            {
                // Логгирование ошибки
                _logger.LogError(ex, "Ошибка при запросе логов");
                return RedirectToAction("Index"); // Или любая другая обработка ошибки
            }
        }
    }
}