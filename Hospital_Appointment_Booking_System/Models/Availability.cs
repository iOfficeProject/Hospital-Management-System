using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class Availability
    {
        public Availability()
        {
            Slots = new HashSet<Slot>();
        }

        public int AvailabilityId { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
    }
}
