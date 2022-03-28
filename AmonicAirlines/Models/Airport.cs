using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class Airport
    {
        public Airport()
        {
            RouteArrivalAirports = new HashSet<Route>();
            RouteDepartureAirports = new HashSet<Route>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Iatacode { get; set; }
        public string Name { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Route> RouteArrivalAirports { get; set; }
        public virtual ICollection<Route> RouteDepartureAirports { get; set; }
    }
}
