using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class Specialization
    {
        public Specialization()
        {
            Users = new HashSet<User>();
        }

        public int SpecializationId { get; set; }
        public string? SpecializationName { get; set; }
        public int? HospitalId { get; set; }

        public virtual Hospital? Hospital { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
