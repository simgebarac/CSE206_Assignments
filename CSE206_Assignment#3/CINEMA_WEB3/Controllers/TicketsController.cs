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
    public class TicketsController : Controller
    {
        private readonly CinemaContext _context;

        public TicketsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.Tickets.Include(t => t.Showtime).Include(t => t.User);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Showtime)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["ShowtimeId"] = new SelectList(_context.Showtimes.Include(s => s.Movie), "ShowtimeId", "StartTime");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username"); 
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,UserId,ShowtimeId,SeatNumber,PurchaseDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Ticket added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while adding the ticket: {ex.Message}";
                }
            }
            ViewData["ShowtimeId"] = new SelectList(_context.Showtimes.Include(s => s.Movie), "ShowtimeId", "StartTime", ticket.ShowtimeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ShowtimeId"] = new SelectList(_context.Showtimes.Include(s => s.Movie), "ShowtimeId", "StartTime", ticket.ShowtimeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", ticket.UserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,UserId,ShowtimeId,SeatNumber,PurchaseDate")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Ticket updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
                    {
                        TempData["ErrorMessage"] = "Ticket not found for update.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "A concurrency error occurred while updating the ticket.";
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while updating the ticket: {ex.Message}";
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowtimeId"] = new SelectList(_context.Showtimes.Include(s => s.Movie), "ShowtimeId", "StartTime", ticket.ShowtimeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Showtime)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                TempData["ErrorMessage"] = "Tickets entity set is null.";
                return Problem("Entity set 'CinemaContext.Tickets' is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                try
                {
                    _context.Tickets.Remove(ticket);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Ticket deleted successfully!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while deleting the ticket: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Ticket to be deleted not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return (_context.Tickets?.Any(e => e.TicketId == id)).GetValueOrDefault();
        }
    }
}