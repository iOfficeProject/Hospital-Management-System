namespace Hospital_Appointment_Booking_System.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? IsPaid { get; set; }
        public AppointmentDTO Appointment { get; set; }
        public UserDTO User { get; set; }
    }
}
