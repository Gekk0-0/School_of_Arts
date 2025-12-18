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
    public class TeachersController : Controller
    {
        private readonly SchoolOfArtsContext _context;

        public TeachersController(SchoolOfArtsContext context)
        {
            _context = context;
        }

        // --- ГОЛОВНИЙ МЕТОД INDEX (З ПОШУКОМ І ФІЛЬТРАМИ) ---
        public async Task<IActionResult> Index(
            string searchName,
            decimal? minSalary,
            decimal? maxSalary,
            DateOnly? dateFrom,
            DateOnly? dateTo)
        {
            // 1. Починаємо формувати запит
            var teachers = _context.Teachers.AsQueryable();

            // 2. Фільтр по ПІБ
            if (!string.IsNullOrEmpty(searchName))
            {
                teachers = teachers.Where(t => t.FullName.Contains(searchName));
            }

            // 3. Фільтр по Зарплаті (Від)
            if (minSalary.HasValue)
            {
                teachers = teachers.Where(t => t.Salary >= minSalary.Value);
            }

            // 4. Фільтр по Зарплаті (До)
            if (maxSalary.HasValue)
            {
                teachers = teachers.Where(t => t.Salary <= maxSalary.Value);
            }

            // 5. Фільтр по Даті (З)
            if (dateFrom.HasValue)
            {
                teachers = teachers.Where(t => t.Birthdate >= dateFrom.Value);
            }

            // 6. Фільтр по Даті (По)
            if (dateTo.HasValue)
            {
                teachers = teachers.Where(t => t.Birthdate <= dateTo.Value);
            }

            // Зберігаємо параметри для View
            ViewData["CurrentName"] = searchName;
            ViewData["MinSalary"] = minSalary;
            ViewData["MaxSalary"] = maxSalary;
            ViewData["DateFrom"] = dateFrom?.ToString("yyyy-MM-dd");
            ViewData["DateTo"] = dateTo?.ToString("yyyy-MM-dd");

            // Агрегація (Статистика)
            decimal avgSalary = 0;
            int count = await teachers.CountAsync();
            if (count > 0)
            {
                avgSalary = await teachers.AverageAsync(t => t.Salary);
            }
            ViewData["AverageSalary"] = avgSalary;
            ViewData["Count"] = count;

            return View(await teachers.ToListAsync());
        }

        // --- ДЕТАЛІ ---
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);

            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // --- СТВОРЕННЯ (GET) ---
        // Цей метод відповідає за відкриття сторінки створення
        public IActionResult Create()
        {
            return View();
        }

        // --- СТВОРЕННЯ (POST) ---
        // Цей метод зберігає дані
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,FullName,PhoneNumber,Birthdate,Salary")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // --- РЕДАГУВАННЯ (GET) ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // --- РЕДАГУВАННЯ (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,FullName,PhoneNumber,Birthdate,Salary")] Teacher teacher)
        {
            if (id != teacher.TeacherId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // --- ВИДАЛЕННЯ (GET) ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);

            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // --- ВИДАЛЕННЯ (POST) ---
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }
    }
}