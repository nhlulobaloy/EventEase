using System.Collections.Generic;

namespace EventEase.Models
{
    public class HomeViewModel
    {
        public int TotalVenues { get; set; }
        public int TotalEvents { get; set; }
        public int TotalBookings { get; set; }

        public List<Venue> RecentVenues { get; set; } = new();
        public List<Event> RecentEvents { get; set; } = new();
        public List<Booking> RecentBookings { get; set; } = new();
    }
}