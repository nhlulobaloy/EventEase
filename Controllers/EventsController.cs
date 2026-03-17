using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using System.Threading.Tasks;
using System.Linq;

namespace EventEase.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventEaseContext _context;

        public EventsController(EventEaseContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Event.Include(e => e.Venue).ToListAsync();
            return View(events);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venue, "VenueId", "VenueName");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventName,EventDate,Description,VenueId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, repopulate the dropdown
            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venue, "VenueId", "VenueName", @event.VenueId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Event.FindAsync(id);
            if (@event == null) return NotFound();

            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venue, "VenueId", "VenueName", @event.VenueId);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,Description,VenueId")] Event @event)
        {
            if (id != @event.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venue, "VenueId", "VenueName", @event.VenueId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Event.Include(e => e.Venue)
                              .FirstOrDefaultAsync(e => e.EventId == id);
            if (@event == null) return NotFound();

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event != null)
            {
                _context.Event.Remove(@event);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}