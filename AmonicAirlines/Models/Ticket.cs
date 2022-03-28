using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public int CabinTypeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PassportNumber { get; set; }
        public int PassportCountryId { get; set; }
        public string BookingReference { get; set; }
        public bool Confirmed { get; set; }

        public virtual Cabintype CabinType { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual User User { get; set; }
    }
}
