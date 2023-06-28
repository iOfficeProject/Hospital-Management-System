using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class MasterRole
    {
        public MasterRole()
        {
            MasterUsers = new HashSet<MasterUser>();
        }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<MasterUser> MasterUsers { get; set; }
    }
}
