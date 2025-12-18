using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_of_arts.Models;

namespace School_of_arts.Controllers
{
    public class PupilsController : Controller
    {
        private readonly SchoolOfArtsContext _context;

        public PupilsController(SchoolOfArtsContext context)
        {
            _context = context;
        }

        // --- ГОЛОВНИЙ МЕТОД INDEX (З ФІЛЬТРАЦІЄЮ) ---
        public async Task<IActionResult> Index(
            string searchName,
            int? classId,
            DateOnly? dateFrom,
            DateOnly? dateTo)
        {
            // 1. Формуємо запит
            var pupils = _context.Pupils
                .Include(p => p.Class)
                .AsQueryable();

            // 2. Фільтр по ПІБ
            if (!string.IsNullOrEmpty(searchName))
            {
                pupils = pupils.Where(p => p.FullName.Contains(searchName));
            }

            // 3. Фільтр по Класу
            if (classId.HasValue)
            {
                pupils = pupils.Where(p => p.ClassId == classId);
            }

            // 4. Фільтр по Даті народження (діапазон)
            if (dateFrom.HasValue)
            {
                pupils = pupils.Where(p => p.Birthdate >= dateFrom.Value);
            }
            if (dateTo.HasValue)
            {
                pupils = pupils.Where(p => p.Birthdate <= dateTo.Value);
            }

            // --- Завантажуємо списки для View ---

            // Список класів для випадаючого меню
            // Використовуємо ClassName для відображення назви
            var classes = await _context.Classes.OrderBy(c => c.ClassName).ToListAsync();
            ViewData["ClassList"] = new SelectList(classes, "ClassId", "ClassName", classId);

            // Зберігаємо введені дані фільтрів
            ViewData["CurrentName"] = searchName;
            ViewData["DateFrom"] = dateFrom?.ToString("yyyy-MM-dd");
            ViewData["DateTo"] = dateTo?.ToString("yyyy-MM-dd");

            // Лічильник знайдених
            ViewData["Count"] = await pupils.CountAsync();

            return View(await pupils.ToListAsync());
        }

        // GET: Pupils/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pupil = await _context.Pupils
                .Include(p => p.Class)
                .FirstOrDefaultAsync(m => m.PupilId == id);

            if (pupil == null) return NotFound();

            return View(pupil);
        }

        // GET: Pupils/Create
        public IActionResult Create()
        {
            // Тут використовуємо ClassName, щоб у списку були назви, а не ID
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassName");
            return View();
        }

        // POST: Pupils/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PupilId,FullName,ClassId,PhoneNumber,Birthdate")] Pupil pupil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pupil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassName", pupil.ClassId);
            return View(pupil);
        }

        // GET: Pupils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pupil = await _context.Pupils.FindAsync(id);
            if (pupil == null) return NotFound();

            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassName", pupil.ClassId);
            return View(pupil);
        }

        // POST: Pupils/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PupilId,FullName,ClassId,PhoneNumber,Birthdate")] Pupil pupil)
        {
            if (id != pupil.PupilId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pupil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PupilExists(pupil.PupilId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassName", pupil.ClassId);
            return View(pupil);
        }

        // GET: Pupils/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pupil = await _context.Pupils
                .Include(p => p.Class)
                .FirstOrDefaultAsync(m => m.PupilId == id);

            if (pupil == null) return NotFound();

            return View(pupil);
        }

        // POST: Pupils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pupil = await _context.Pupils.FindAsync(id);
            if (pupil != null) _context.Pupils.Remove(pupil);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PupilExists(int id)
        {
            return _context.Pupils.Any(e => e.PupilId == id);
        }
    }
}