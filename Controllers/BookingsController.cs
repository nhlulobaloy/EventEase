using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using System.Threading.Tasks;
using System.Linq;

namespace EventEase.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEaseContext _context;

        public BookingsController(EventEaseContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Booking
                                         .Include(b => b.Event)
                                         .Include(b => b.Venue)
                                         .ToListAsync();
            return View(bookings);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Event, "EventId", "EventName");
            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venue, "VenueId", "VenueName");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,VenueId,BookingDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Prevent double booking for the same event & venue
                bool exists = await _context.Booking
                    .AnyAsync(b => b.EventId == booking.EventId && b.VenueId == booking.VenueId);

                if (exists)
                {
                    ModelState.AddModelError("", "This event is already booked for this venue.");
                }
                else
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["EventId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Booking
                                        .Include(b => b.Event)
                                        .Include(b => b.Venue)
                                        .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}