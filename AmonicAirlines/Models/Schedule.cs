using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class Schedule
    {
        public Schedule()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int AircraftId { get; set; }
        public int RouteId { get; set; }
        public double EconomyPrice { get; set; }
        public bool Confirmed { get; set; }
        public string FlightNumber { get; set; }

        public virtual Aircraft Aircraft { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
