using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class User
    {
        public User()
        {
            Tickets = new HashSet<Ticket>();
            Trackings = new HashSet<Tracking>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? OfficeId { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool? Active { get; set; }

        public virtual Office Office { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
    }
}
