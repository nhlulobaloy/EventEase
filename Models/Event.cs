using System;
using System.Collections.Generic;

namespace EventEase.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public string Description { get; set; } = null!;

        // Foreign key
        public int VenueId { get; set; }

        // Navigation property should be nullable for create
        public Venue? Venue { get; set; }

        // Bookings can be empty on create
        public ICollection<Booking>? Bookings { get; set; }
    }
}