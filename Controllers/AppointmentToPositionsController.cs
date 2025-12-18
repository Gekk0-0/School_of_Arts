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
    public class AppointmentToPositionsController : Controller
    {
        private readonly SchoolOfArtsContext _context;

        public AppointmentToPositionsController(SchoolOfArtsContext context)
        {
            _context = context;
        }

        // GET: AppointmentToPositions
        public async Task<IActionResult> Index()
        {
            var schoolOfArtsContext = _context.AppointmentToPositions.Include(a => a.Post).Include(a => a.Teacher);
            return View(await schoolOfArtsContext.ToListAsync());
        }

        // GET: AppointmentToPositions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentToPosition = await _context.AppointmentToPositions
                .Include(a => a.Teacher)
                .Include(a => a.Post)
                .FirstOrDefaultAsync(m => m.AppointmentToPositionId == id);
            if (appointmentToPosition == null)
            {
                return NotFound();
            }

            return View(appointmentToPosition);
        }

        // GET: AppointmentToPositions/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Positions, "PositionId", "PositionId");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId");
            return View();
        }

        // POST: AppointmentToPositions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentToPositionId,PositionStatus,AppointmentDate,DismissalDate,TeacherId,PostId")] AppointmentToPosition appointmentToPosition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentToPosition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.Positions, "PositionId", "PositionId", appointmentToPosition.PostId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId", appointmentToPosition.TeacherId);
            return View(appointmentToPosition);
        }

        // GET: AppointmentToPositions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentToPosition = await _context.AppointmentToPositions.FindAsync(id);
            if (appointmentToPosition == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Positions, "PositionId", "PositionId", appointmentToPosition.PostId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId", appointmentToPosition.TeacherId);
            return View(appointmentToPosition);
        }

        // POST: AppointmentToPositions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentToPositionId,PositionStatus,AppointmentDate,DismissalDate,TeacherId,PostId")] AppointmentToPosition appointmentToPosition)
        {
            if (id != appointmentToPosition.AppointmentToPositionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentToPosition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentToPositionExists(appointmentToPosition.AppointmentToPositionId))
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
            ViewData["PostId"] = new SelectList(_context.Positions, "PositionId", "PositionId", appointmentToPosition.PostId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId", appointmentToPosition.TeacherId);
            return View(appointmentToPosition);
        }

        // GET: AppointmentToPositions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentToPosition = await _context.AppointmentToPositions
                .Include(a => a.Post)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(m => m.AppointmentToPositionId == id);
            if (appointmentToPosition == null)
            {
                return NotFound();
            }

            return View(appointmentToPosition);
        }

        // POST: AppointmentToPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentToPosition = await _context.AppointmentToPositions.FindAsync(id);
            if (appointmentToPosition != null)
            {
                _context.AppointmentToPositions.Remove(appointmentToPosition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentToPositionExists(int id)
        {
            return _context.AppointmentToPositions.Any(e => e.AppointmentToPositionId == id);
        }
    }
}
