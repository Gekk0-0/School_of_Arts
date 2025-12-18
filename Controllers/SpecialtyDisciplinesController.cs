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
    public class SpecialtyDisciplinesController : Controller
    {
        private readonly SchoolOfArtsContext _context;

        public SpecialtyDisciplinesController(SchoolOfArtsContext context)
        {
            _context = context;
        }

        // GET: SpecialtyDisciplines
        public async Task<IActionResult> Index()
        {
            var schoolOfArtsContext = _context.SpecialtyDisciplines.Include(s => s.Department).Include(s => s.Discipline);
            return View(await schoolOfArtsContext.ToListAsync());
        }

        // GET: SpecialtyDisciplines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialtyDiscipline = await _context.SpecialtyDisciplines
                .Include(s => s.Department)
                .Include(s => s.Discipline)
                .FirstOrDefaultAsync(m => m.SpecialtyDisciplineId == id);
            if (specialtyDiscipline == null)
            {
                return NotFound();
            }

            return View(specialtyDiscipline);
        }

        // GET: SpecialtyDisciplines/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId");
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineId");
            return View();
        }

        // POST: SpecialtyDisciplines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecialtyDisciplineId,DepartmentId,DisciplineId")] SpecialtyDiscipline specialtyDiscipline)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialtyDiscipline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", specialtyDiscipline.DepartmentId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineId", specialtyDiscipline.DisciplineId);
            return View(specialtyDiscipline);
        }

        // GET: SpecialtyDisciplines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialtyDiscipline = await _context.SpecialtyDisciplines.FindAsync(id);
            if (specialtyDiscipline == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", specialtyDiscipline.DepartmentId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineId", specialtyDiscipline.DisciplineId);
            return View(specialtyDiscipline);
        }

        // POST: SpecialtyDisciplines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecialtyDisciplineId,DepartmentId,DisciplineId")] SpecialtyDiscipline specialtyDiscipline)
        {
            if (id != specialtyDiscipline.SpecialtyDisciplineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialtyDiscipline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyDisciplineExists(specialtyDiscipline.SpecialtyDisciplineId))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", specialtyDiscipline.DepartmentId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineId", specialtyDiscipline.DisciplineId);
            return View(specialtyDiscipline);
        }

        // GET: SpecialtyDisciplines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialtyDiscipline = await _context.SpecialtyDisciplines
                .Include(s => s.Department)
                .Include(s => s.Discipline)
                .FirstOrDefaultAsync(m => m.SpecialtyDisciplineId == id);
            if (specialtyDiscipline == null)
            {
                return NotFound();
            }

            return View(specialtyDiscipline);
        }

        // POST: SpecialtyDisciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialtyDiscipline = await _context.SpecialtyDisciplines.FindAsync(id);
            if (specialtyDiscipline != null)
            {
                _context.SpecialtyDisciplines.Remove(specialtyDiscipline);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtyDisciplineExists(int id)
        {
            return _context.SpecialtyDisciplines.Any(e => e.SpecialtyDisciplineId == id);
        }
    }
}
