using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? IsPaid { get; set; }
        public int? AppointmentId { get; set; }
        public int? UserId { get; set; }

        public virtual Appointment? Appointment { get; set; }
        public virtual User? User { get; set; }
    }
}
