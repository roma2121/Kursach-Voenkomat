using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kursach_Voenkomat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<TableInfo> tables = GetTableListFromDatabase();
            return View(tables);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Audit()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<TableInfo> GetTableListFromDatabase()
        {
            return new List<TableInfo>
            {
                new TableInfo { TableName = "ВидыСтатусовЯвки", DisplayName = "Виды статусов явки" },
                new TableInfo { TableName = "ВоенноПрофессиональныеНаправленности", DisplayName = "Военно-профессиональные направленности" },
                new TableInfo { TableName = "Врачи", DisplayName = "Врачи" },
                new TableInfo { TableName = "ЗаключенияВрачей", DisplayName = "Заключения врачей" },
                new TableInfo { TableName = "КатегорииГодности", DisplayName = "Категории годности" },
                new TableInfo { TableName = "КатегорииПрофессиональнойПригодности", DisplayName = "Категории профессиональной пригодности" },
                new TableInfo { TableName = "МедицинскиеДокументы", DisplayName = "Медицинские документы" },
                new TableInfo { TableName = "Образование", DisplayName = "Образование" },
                new TableInfo { TableName = "ОбразовательныеОрганизации", DisplayName = "Образовательные организации" },
                new TableInfo { TableName = "Паспорта", DisplayName = "Паспорта" },
                new TableInfo { TableName = "Повестки", DisplayName = "Повестки" },
                new TableInfo { TableName = "Полы", DisplayName = "Полы" },
                new TableInfo { TableName = "Призывники", DisplayName = "Призывники" },
                new TableInfo { TableName = "ПризывникРабота", DisplayName = "Призывник-работа" },
                new TableInfo { TableName = "ПрофессиональноПсихологическиеПоказателиГраждан", DisplayName = "Профессионально-психологические показатели граждан" },
                new TableInfo { TableName = "Работа", DisplayName = "Работа" },
                new TableInfo { TableName = "РешенияКомиссии", DisplayName = "Решения комиссии" },
                new TableInfo { TableName = "СпециальностиВрачей", DisplayName = "Специальности врачей" },
                new TableInfo { TableName = "ТипыОбразовательныхОрганизаций", DisplayName = "Типы образовательных организаций" },
                new TableInfo { TableName = "ТипыПовесток", DisplayName = "Типы повесток" },
                new TableInfo { TableName = "УровниОбразования", DisplayName = "Уровни образования" }
            };

        }

        public IActionResult RedirectToTable(string tableName)
        {
            return RedirectToAction("Index", tableName);
        }
    }
}