using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Data;
using Kursach_Voenkomat.Models;

namespace Kursach_Voenkomat
{
    public class МедицинскиеДокументыController : Controller
    {
        private readonly ApplicationDbContext _context;

        public МедицинскиеДокументыController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: МедицинскиеДокументы
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.МедицинскиеДокументы.Include(м => м.Призывник);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: МедицинскиеДокументы/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.МедицинскиеДокументы == null)
            {
                return NotFound();
            }

            var медицинскиеДокументы = await _context.МедицинскиеДокументы
                .Include(м => м.Призывник)
                .FirstOrDefaultAsync(m => m.ID_медицинского_документа == id);
            if (медицинскиеДокументы == null)
            {
                return NotFound();
            }

            return View(медицинскиеДокументы);
        }

        // GET: МедицинскиеДокументы/Create
        public IActionResult Create()
        {
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО");
            return View();
        }

        // POST: МедицинскиеДокументы/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_медицинского_документа,ID_призывника,Диагноз,Наименование_медицинской_организации,Дата_выписки_медицинского_заключения")] МедицинскиеДокументы медицинскиеДокументы)
        {
            if (ModelState.IsValid)
            {
                _context.Add(медицинскиеДокументы);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", медицинскиеДокументы.ID_призывника);
            return View(медицинскиеДокументы);
        }

        // GET: МедицинскиеДокументы/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.МедицинскиеДокументы == null)
            {
                return NotFound();
            }

            var медицинскиеДокументы = await _context.МедицинскиеДокументы.FindAsync(id);
            if (медицинскиеДокументы == null)
            {
                return NotFound();
            }
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "ПризывникФИО", медицинскиеДокументы.ID_призывника);
            return View(медицинскиеДокументы);
        }

        // POST: МедицинскиеДокументы/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_медицинского_документа,ID_призывника,Диагноз,Наименование_медицинской_организации,Дата_выписки_медицинского_заключения")] МедицинскиеДокументы медицинскиеДокументы)
        {
            if (id != медицинскиеДокументы.ID_медицинского_документа)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(медицинскиеДокументы);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!МедицинскиеДокументыExists(медицинскиеДокументы.ID_медицинского_документа))
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
            ViewData["ID_призывника"] = new SelectList(_context.Призывники, "ID_призывника", "Имя", медицинскиеДокументы.ID_призывника);
            return View(медицинскиеДокументы);
        }

        // GET: МедицинскиеДокументы/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.МедицинскиеДокументы == null)
            {
                return NotFound();
            }

            var медицинскиеДокументы = await _context.МедицинскиеДокументы
                .Include(м => м.Призывник)
                .FirstOrDefaultAsync(m => m.ID_медицинского_документа == id);
            if (медицинскиеДокументы == null)
            {
                return NotFound();
            }

            return View(медицинскиеДокументы);
        }

        // POST: МедицинскиеДокументы/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.МедицинскиеДокументы == null)
            {
                return Problem("Entity set 'ApplicationDbContext.МедицинскиеДокументы'  is null.");
            }
            var медицинскиеДокументы = await _context.МедицинскиеДокументы.FindAsync(id);
            if (медицинскиеДокументы != null)
            {
                _context.МедицинскиеДокументы.Remove(медицинскиеДокументы);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool МедицинскиеДокументыExists(int id)
        {
          return (_context.МедицинскиеДокументы?.Any(e => e.ID_медицинского_документа == id)).GetValueOrDefault();
        }
    }
}
