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
    public class ClassesController : Controller
    {
        private readonly SchoolOfArtsContext _context;

        public ClassesController(SchoolOfArtsContext context)
        {
            _context = context;
        }

        // --- ГОЛОВНИЙ МЕТОД INDEX (З ФІЛЬТРАМИ) ---
        public async Task<IActionResult> Index(
            string searchName,
            int? departmentId,
            int? curatorId)
        {
            // 1. Формуємо запит із необхідними зв'язками
            var classes = _context.Classes
                .Include(c => c.Curator)
                .Include(c => c.Department)
                .Include(c => c.Specialty)
                    .ThenInclude(s => s.Discipline) // Щоб дістати назву предмету
                .AsQueryable();

            // 2. Фільтр по Назві класу
            if (!string.IsNullOrEmpty(searchName))
            {
                classes = classes.Where(c => c.ClassName.Contains(searchName));
            }

            // 3. Фільтр по Відділу
            if (departmentId.HasValue)
            {
                classes = classes.Where(c => c.DepartmentId == departmentId);
            }

            // 4. Фільтр по Куратору
            if (curatorId.HasValue)
            {
                classes = classes.Where(c => c.CuratorId == curatorId);
            }

            // --- Завантажуємо списки для випадаючих меню ---

            // Список відділів
            var departments = await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync();
            ViewData["DepartmentList"] = new SelectList(departments, "DepartmentId", "DepartmentName", departmentId);

            // Список кураторів (вчителів)
            var curators = await _context.Teachers.OrderBy(t => t.FullName).ToListAsync();
            ViewData["CuratorList"] = new SelectList(curators, "TeacherId", "FullName", curatorId);

            // Зберігаємо поточні значення фільтрів
            ViewData["CurrentName"] = searchName;
            ViewData["Count"] = await classes.CountAsync();

            return View(await classes.ToListAsync());
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes
                .Include(c => c.Curator)
                .Include(c => c.Specialty)
                    .ThenInclude(s => s.Discipline)
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (@class == null) return NotFound();

            return View(@class);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            // Увага: використовуємо імена полів (FullName, DepartmentName) для відображення
            ViewData["CuratorId"] = new SelectList(_context.Teachers, "TeacherId", "FullName");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            // Для спеціальності складніше, поки виводимо ID або треба робити окремий Select
            ViewData["SpecialtyId"] = new SelectList(_context.SpecialtyDisciplines, "SpecialtyDisciplineId", "SpecialtyDisciplineId");
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassId,ClassName,CuratorId,StudyYear,SpecialtyId,DepartmentId,TermYears")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CuratorId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", @class.CuratorId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", @class.DepartmentId);
            ViewData["SpecialtyId"] = new SelectList(_context.SpecialtyDisciplines, "SpecialtyDisciplineId", "SpecialtyDisciplineId", @class.SpecialtyId);
            return View(@class);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null) return NotFound();

            ViewData["CuratorId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", @class.CuratorId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", @class.DepartmentId);
            ViewData["SpecialtyId"] = new SelectList(_context.SpecialtyDisciplines, "SpecialtyDisciplineId", "SpecialtyDisciplineId", @class.SpecialtyId);
            return View(@class);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassId,ClassName,CuratorId,StudyYear,SpecialtyId,DepartmentId,TermYears")] Class @class)
        {
            if (id != @class.ClassId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.ClassId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CuratorId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", @class.CuratorId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", @class.DepartmentId);
            ViewData["SpecialtyId"] = new SelectList(_context.SpecialtyDisciplines, "SpecialtyDisciplineId", "SpecialtyDisciplineId", @class.SpecialtyId);
            return View(@class);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes
                .Include(c => c.Curator)
                .Include(c => c.Department)
                .Include(c => c.Specialty)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (@class == null) return NotFound();

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null) _context.Classes.Remove(@class);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ClassId == id);
        }
    }
}