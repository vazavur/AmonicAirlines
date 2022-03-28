using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class Country
    {
        public Country()
        {
            Airports = new HashSet<Airport>();
            Offices = new HashSet<Office>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Airport> Airports { get; set; }
        public virtual ICollection<Office> Offices { get; set; }
    }
}
