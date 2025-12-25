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
    public class TheatersController : Controller
    {
        private readonly CinemaContext _context;

        public TheatersController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Theaters
        public async Task<IActionResult> Index()
        {
            return _context.Theaters != null ?
                            View(await _context.Theaters.ToListAsync()) :
                            Problem("Entity set 'CinemaContext.Theaters' is null.");
        }

        // GET: Theaters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Theaters == null)
            {
                return NotFound();
            }

            var theater = await _context.Theaters
                .FirstOrDefaultAsync(m => m.TheaterId == id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // GET: Theaters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Theaters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TheaterId,Name,SeatCapacity")] Theater theater)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(theater);
                    await _context.SaveChangesAsync();
                    // Başarı mesajı ekle
                    TempData["SuccessMessage"] = "Theater added successfully!"; 
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Hata mesajı ekle
                    TempData["ErrorMessage"] = $"An error occurred while adding the theater: {ex.Message}"; 
                }
            }
            return View(theater);
        }

        // GET: Theaters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Theaters == null)
            {
                return NotFound();
            }

            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
            {
                return NotFound();
            }
            return View(theater);
        }

        // POST: Theaters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TheaterId,Name,SeatCapacity")] Theater theater)
        {
            if (id != theater.TheaterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theater);
                    await _context.SaveChangesAsync();
                    // Başarı mesajı ekle
                    TempData["SuccessMessage"] = "Theater updated successfully!"; 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheaterExists(theater.TheaterId))
                    {
                        TempData["ErrorMessage"] = "Theater not found for update.";  
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "A concurrency error occurred while updating the theater.";  
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while updating the theater: {ex.Message}"; 
                }
                return RedirectToAction(nameof(Index));
            }
            return View(theater);
        }

        // GET: Theaters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Theaters == null)
            {
                return NotFound();
            }

            var theater = await _context.Theaters
                .FirstOrDefaultAsync(m => m.TheaterId == id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // POST: Theaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Theaters == null)
            {
                TempData["ErrorMessage"] = "Theaters entity set is null."; 
                return Problem("Entity set 'CinemaContext.Theaters' is null.");
            }
            var theater = await _context.Theaters.FindAsync(id);
            if (theater != null)
            {
                try
                {
                    _context.Theaters.Remove(theater);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Theater deleted successfully!"; 
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while deleting the theater: {ex.Message}"; 
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Theater to be deleted not found."; 
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TheaterExists(int id)
        {
            return (_context.Theaters?.Any(e => e.TheaterId == id)).GetValueOrDefault();
        }
    }
}