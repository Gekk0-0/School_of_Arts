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
    public class AffiliationWithDepartmentsController : Controller
    {
        private readonly SchoolOfArtsContext _context;

        public AffiliationWithDepartmentsController(SchoolOfArtsContext context)
        {
            _context = context;
        }

        // --- ОНОВЛЕНИЙ МЕТОД INDEX (З ФІЛЬТРАЦІЄЮ) ---
        public async Task<IActionResult> Index(
            int? teacherId,
            int? disciplineId,
            string occupationType)
        {
            // 1. Початковий запит з підтягуванням зв'язків
            var affiliations = _context.AffiliationWithDepartments
                .Include(a => a.Discipline)
                .Include(a => a.Teacher)
                .AsQueryable();

            // 2. Фільтр по Викладачу
            if (teacherId.HasValue)
            {
                affiliations = affiliations.Where(a => a.TeacherId == teacherId);
            }

            // 3. Фільтр по Дисципліні
            if (disciplineId.HasValue)
            {
                affiliations = affiliations.Where(a => a.DisciplineId == disciplineId);
            }

            // 4. Фільтр по Типу зайнятості (текстовий пошук)
            if (!string.IsNullOrEmpty(occupationType))
            {
                affiliations = affiliations.Where(a => a.OccupationType.Contains(occupationType));
            }

            // --- Заповнення списків для фільтрів у View ---

            // Список викладачів (сортуємо по імені)
            var teachers = await _context.Teachers.OrderBy(t => t.FullName).ToListAsync();
            // "TeacherId" - це значення, "FullName" - це текст у списку
            ViewData["TeacherList"] = new SelectList(teachers, "TeacherId", "FullName", teacherId);

            // Список дисциплін
            var disciplines = await _context.Disciplines.OrderBy(d => d.SubjectName).ToListAsync();
            // "DisciplineId" - значення, "SubjectName" - текст
            ViewData["DisciplineList"] = new SelectList(disciplines, "DisciplineId", "SubjectName", disciplineId);

            // Зберігаємо поточний пошук та кількість знайдених
            ViewData["CurrentOccupation"] = occupationType;
            ViewData["Count"] = await affiliations.CountAsync();

            return View(await affiliations.ToListAsync());
        }

        // GET: AffiliationWithDepartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var affiliationWithDepartment = await _context.AffiliationWithDepartments
                .Include(a => a.Discipline)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(m => m.AffiliationWithDepartmentId == id);

            if (affiliationWithDepartment == null) return NotFound();

            return View(affiliationWithDepartment);
        }

        // GET: AffiliationWithDepartments/Create
        public IActionResult Create()
        {
            // ВИПРАВЛЕНО: Тепер показує імена/назви замість ID
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "SubjectName");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName");
            return View();
        }

        // POST: AffiliationWithDepartments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AffiliationWithDepartmentId,WageRate,OccupationType,TeacherId,DisciplineId")] AffiliationWithDepartment affiliationWithDepartment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(affiliationWithDepartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ВИПРАВЛЕНО: SubjectName та FullName
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "SubjectName", affiliationWithDepartment.DisciplineId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", affiliationWithDepartment.TeacherId);
            return View(affiliationWithDepartment);
        }

        // GET: AffiliationWithDepartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var affiliationWithDepartment = await _context.AffiliationWithDepartments.FindAsync(id);
            if (affiliationWithDepartment == null) return NotFound();

            // ВИПРАВЛЕНО: SubjectName та FullName
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "SubjectName", affiliationWithDepartment.DisciplineId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", affiliationWithDepartment.TeacherId);
            return View(affiliationWithDepartment);
        }

        // POST: AffiliationWithDepartments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AffiliationWithDepartmentId,WageRate,OccupationType,TeacherId,DisciplineId")] AffiliationWithDepartment affiliationWithDepartment)
        {
            if (id != affiliationWithDepartment.AffiliationWithDepartmentId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(affiliationWithDepartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AffiliationWithDepartmentExists(affiliationWithDepartment.AffiliationWithDepartmentId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            // ВИПРАВЛЕНО: SubjectName та FullName
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "SubjectName", affiliationWithDepartment.DisciplineId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", affiliationWithDepartment.TeacherId);
            return View(affiliationWithDepartment);
        }

        // GET: AffiliationWithDepartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var affiliationWithDepartment = await _context.AffiliationWithDepartments
                .Include(a => a.Discipline)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(m => m.AffiliationWithDepartmentId == id);

            if (affiliationWithDepartment == null) return NotFound();

            return View(affiliationWithDepartment);
        }

        // POST: AffiliationWithDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var affiliationWithDepartment = await _context.AffiliationWithDepartments.FindAsync(id);
            if (affiliationWithDepartment != null)
            {
                _context.AffiliationWithDepartments.Remove(affiliationWithDepartment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AffiliationWithDepartmentExists(int id)
        {
            return _context.AffiliationWithDepartments.Any(e => e.AffiliationWithDepartmentId == id);
        }
    }
}