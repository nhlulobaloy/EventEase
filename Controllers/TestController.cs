using EventEase.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventEase.Controllers
{
    public class TestController : Controller
    {
        private readonly EventEaseContext _context;
        public TestController(EventEaseContext context) => _context = context;

        public IActionResult Index() => Content($"Venues: {_context.Venue.Count()}");
    }
}
