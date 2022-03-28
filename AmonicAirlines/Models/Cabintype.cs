using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class Cabintype
    {
        public Cabintype()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
