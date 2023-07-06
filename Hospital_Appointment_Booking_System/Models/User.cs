using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class User
    {
        public User()
        {
            Appointments = new HashSet<Appointment>();
            Availabilities = new HashSet<Availability>();
            Payments = new HashSet<Payment>();
            Slots = new HashSet<Slot>();
        }

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public long? MobileNumber { get; set; }
        public int? RoleId { get; set; }
        public int? SpecializationId { get; set; }
        public int? HospitalId { get; set; }

        public virtual Hospital? Hospital { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Specialization? Specialization { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
    }
}
