using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class MasterUser
    {
        public MasterUser()
        {
            Hospitals = new HashSet<Hospital>();
        }

        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public long? MobileNumber { get; set; }
        public int? RoleId { get; set; }

        public virtual MasterRole? Role { get; set; }
        public virtual ICollection<Hospital> Hospitals { get; set; }
    }
}
