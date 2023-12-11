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
    }
}
