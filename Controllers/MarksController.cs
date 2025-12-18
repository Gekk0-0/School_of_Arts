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
    public class MarksController : Controller
    {
        private readonly SchoolOfArtsContext _context;

        public MarksController(SchoolOfArtsContext context)
        {
            _context = context;
        }

        // GET: Marks
        public async Task<IActionResult> Index(string searchString, int? disciplineId)
        {
            var marksQuery = _context.Marks
                .Include(m => m.Discipline)
                .Include(m => m.Pupil)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                marksQuery = marksQuery.Where(m => m.Pupil.FullName.Contains(searchString));
            }

            if (disciplineId.HasValue && disciplineId != 0)
            {
                marksQuery = marksQuery.Where(m => m.DisciplineId == disciplineId);
            }

            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "SubjectName", disciplineId);
            ViewData["CurrentFilter"] = searchString;

            return View(await marksQuery.ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Discipline)
                .Include(m => m.Pupil)
                .FirstOrDefaultAsync(m => m.MarkId == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: Marks/Create
        public IActionResult Create()
        {
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineId");
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "PupilId");
            return View();
        }

        // POST: Marks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarkId,Mark1,IsPresent,DisciplineId,PupilId,RatingDate")] Mark mark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineId", mark.DisciplineId);
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "PupilId", mark.PupilId);
            return View(mark);
        }

        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineId", mark.DisciplineId);
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "PupilId", mark.PupilId);
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarkId,Mark1,IsPresent,DisciplineId,PupilId,RatingDate")] Mark mark)
        {
            if (id != mark.MarkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkExists(mark.MarkId))
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
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineId", mark.DisciplineId);
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "PupilId", mark.PupilId);
            return View(mark);
        }

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Discipline)
                .Include(m => m.Pupil)
                .FirstOrDefaultAsync(m => m.MarkId == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mark = await _context.Marks.FindAsync(id);
            if (mark != null)
            {
                _context.Marks.Remove(mark);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkExists(int id)
        {
            return _context.Marks.Any(e => e.MarkId == id);
        }
    }
}
