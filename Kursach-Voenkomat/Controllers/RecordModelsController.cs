using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kursach_Voenkomat.Controllers
{
    public class RecordModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecordModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RecordModel record)
        {
            // Проверка наличия записи с такими же данными
            var записьВБазе = _context.RecordModel.FirstOrDefault(p =>
                p.Фамилия == record.Фамилия &&
                p.Имя == record.Имя &&
                p.Отчество == record.Отчество &&
                p.Дата_рождения == record.Дата_рождения &&
                p.Дата_посещения == record.Дата_посещения);

            if (записьВБазе != null)
            {
                // Запись уже существует, возвращаем ошибку
                ModelState.AddModelError(string.Empty, "Данные уже существуют в базе");
            }

            // Проверка занятости выбранной даты
            var записьСВыбраннойДатой = _context.RecordModel.FirstOrDefault(p =>
                p.Дата_посещения == record.Дата_посещения);

            if (записьСВыбраннойДатой != null)
            {
                // Дата уже занята, возвращаем ошибку
                ModelState.AddModelError("Дата_посещения", "Выбранная дата уже занята");
            }

            if (ModelState.IsValid)
            {
                // Все остальные действия сохранения в базу данных
                _context.RecordModel.Add(record);
                _context.SaveChanges();
                return RedirectToAction("Confirmation");
            }

            return View(record);
        }
        public IActionResult Confirmation()
        {
            // Получение максимального ID из базы данных
            var последнийИндекс = _context.RecordModel.Max(p => p.ID_посещения);

            // Получение записи по максимальному ID
            var последняяЗапись = _context.RecordModel.FirstOrDefault(p => p.ID_посещения == последнийИндекс);


            // Получение выбранной даты, если последняя запись существует
            DateTime выбраннаяДата = последняяЗапись != null ? последняяЗапись.Дата_посещения : DateTime.Now; // DateTime.Now - просто для примера, может быть другой дефолтной датой


            // Создание модели, в которой передается выбранная дата
            var model = new RecordModel { Дата_посещения = выбраннаяДата };

            return View(model);
        }
    }
}
