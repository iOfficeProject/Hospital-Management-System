using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class Slot
    {
        public Slot()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int SlotId { get; set; }
        public DateTime? SlotDate { get; set; }
        public DateTime? SlotStartTime { get; set; }
        public DateTime? SlotEndTime { get; set; }
        public int? AvailabilityId { get; set; }
        public int? HospitalId { get; set; }
        public int? UserId { get; set; }

        public virtual Availability? Availability { get; set; }
        public virtual Hospital? Hospital { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
