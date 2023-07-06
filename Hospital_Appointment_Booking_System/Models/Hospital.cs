using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class Hospital
    {
        public Hospital()
        {
            Appointments = new HashSet<Appointment>();
            Slots = new HashSet<Slot>();
            Specializations = new HashSet<Specialization>();
            Users = new HashSet<User>();
        }

        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = null!;
        public string Location { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
