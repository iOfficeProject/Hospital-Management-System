using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            Payments = new HashSet<Payment>();
        }

        public int AppointmentId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public DateTime? AppointmentStartTime { get; set; }
        public DateTime? AppointmentEndTime { get; set; }
        public int? SlotId { get; set; }
        public int? HospitalId { get; set; }
        public int? UserId { get; set; }

        public virtual Hospital? Hospital { get; set; }
        public virtual Slot? Slot { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
