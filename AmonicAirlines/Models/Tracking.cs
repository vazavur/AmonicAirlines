using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class Tracking
    {
        public int Id { get; set; }
        public DateTime LoginDateTime { get; set; }
        public DateTime? LogoutDateTime { get; set; }
        public TimeSpan? TimeInSystem { get; set; }
        public string LogoutReason { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
