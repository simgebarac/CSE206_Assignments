using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CINEMA_WEB3.Models;

namespace CINEMA_WEB3.Controllers
{
    public class ShowtimesController : Controller
    {
        private readonly CinemaContext _context;

        public ShowtimesController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Showtimes
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.Showtimes.Include(s => s.Movie).Include(s => s.Theater);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: Showtimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Showtimes == null)
            {
                return NotFound();
            }

            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .FirstOrDefaultAsync(m => m.ShowtimeId == id);
            if (showtime == null)
            {
                return NotFound();
            }

            return View(showtime);
        }

        // GET: Showtimes/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId");
            ViewData["TheaterId"] = new SelectList(_context.Theaters, "TheaterId", "TheaterId");
            return View();
        }

        // POST: Showtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowtimeId,MovieId,TheaterId,StartTime")] Showtime showtime)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(showtime);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Showtime added successfully!"; 
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while adding the showtime: {ex.Message}"; 
                }
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", showtime.MovieId);
            ViewData["TheaterId"] = new SelectList(_context.Theaters, "TheaterId", "TheaterId", showtime.TheaterId);
            return View(showtime);
        }

        // GET: Showtimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Showtimes == null)
            {
                return NotFound();
            }

            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", showtime.MovieId);
            ViewData["TheaterId"] = new SelectList(_context.Theaters, "TheaterId", "TheaterId", showtime.TheaterId);
            return View(showtime);
        }

        // POST: Showtimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowtimeId,MovieId,TheaterId,StartTime")] Showtime showtime)
        {
            if (id != showtime.ShowtimeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(showtime);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Showtime updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowtimeExists(showtime.ShowtimeId))
                    {
                        TempData["ErrorMessage"] = "Showtime not found.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "A concurrency error occurred while updating the showtime.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", showtime.MovieId);
            ViewData["TheaterId"] = new SelectList(_context.Theaters, "TheaterId", "TheaterId", showtime.TheaterId);
            return View(showtime);
        }

        // GET: Showtimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Showtimes == null)
            {
                return NotFound();
            }

            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .FirstOrDefaultAsync(m => m.ShowtimeId == id);
            if (showtime == null)
            {
                return NotFound();
            }

            return View(showtime);
        }

        // POST: Showtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Showtimes == null)
            {
                TempData["ErrorMessage"] = "Showtimes entity set is null.";
                return Problem("Entity set 'CinemaContext.Showtimes'  is null.");
            }
            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime != null)
            {
                try
                {
                    _context.Showtimes.Remove(showtime);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Showtime deleted successfully!"; 
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while deleting the showtime: {ex.Message}"; 
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Showtime to be deleted not found."; 
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowtimeExists(int id)
        {
          return (_context.Showtimes?.Any(e => e.ShowtimeId == id)).GetValueOrDefault();
        }
    }
}
