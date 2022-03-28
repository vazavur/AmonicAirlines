using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class Aircraft
    {
        public Aircraft()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MakeModel { get; set; }
        public int TotalSeats { get; set; }
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
