using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace EventEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventEaseContext _context;

        public HomeController(EventEaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                TotalVenues = await _context.Venue.CountAsync(),
                TotalEvents = await _context.Event.CountAsync(),
                TotalBookings = await _context.Booking.CountAsync(),
                RecentVenues = await _context.Venue.OrderByDescending(v => v.VenueId).Take(5).ToListAsync(),
                RecentEvents = await _context.Event.OrderByDescending(e => e.EventId).Take(5).ToListAsync(),
                RecentBookings = await _context.Booking.OrderByDescending(b => b.BookingId).Take(5).ToListAsync()
            };
            return View(model);
        }
    }
}