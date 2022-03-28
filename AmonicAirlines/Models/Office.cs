using System;
using System.Collections.Generic;

#nullable disable

namespace AmonicAirlines
{
    public partial class Office
    {
        public Office()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Contact { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
