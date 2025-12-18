using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Обов'язково для SelectList
using Microsoft.EntityFrameworkCore;
using School_of_arts.Models;
using System.Linq;
using System.Threading.Tasks;

namespace School_of_arts.Controllers
{
    public class RatingsController : Controller
    {
        private readonly SchoolOfArtsContext _context;

        public RatingsController(SchoolOfArtsContext context)
        {
            _context = context;
        }

        // Додаємо параметр classId для прийому вибору з форми
        public async Task<IActionResult> Index(int? classId)
        {
            // 1. ЗАВАНТАЖУЄМО СПИСОК КЛАСІВ ДЛЯ ВИПАДАЮЧОГО МЕНЮ
            var classes = await _context.Classes
                .OrderBy(c => c.ClassName) // Сортуємо по назві
                .ToListAsync();

            // Передаємо у View. "ClassId" - це ID, "ClassName" - це текст, який бачить юзер
            ViewData["ClassList"] = new SelectList(classes, "ClassId", "ClassName", classId);

            // 2. ПОЧИНАЄМО ФОРМУВАТИ ЗАПИТ
            var pupilsQuery = _context.Pupils
                .Include(p => p.Class)
                .Include(p => p.Marks)
                .AsQueryable();

            // 3. ЯКЩО ОБРАНО КЛАС — ФІЛЬТРУЄМО
            if (classId.HasValue)
            {
                pupilsQuery = pupilsQuery.Where(p => p.ClassId == classId);
            }

            // 4. ОБЧИСЛЮЄМО РЕЙТИНГ
            var ratingData = await pupilsQuery
                .Select(p => new PupilRatingViewModel
                {
                    PupilId = p.PupilId,
                    FullName = p.FullName,
                    ClassName = p.Class.ClassName,
                    // Рахуємо середній бал
                    AverageMark = p.Marks.Any() ? p.Marks.Average(m => m.Mark1) : 0
                })
                .OrderByDescending(x => x.AverageMark) // Сортуємо: відмінники зверху
                .ToListAsync();

            return View(ratingData);
        }
    }
}