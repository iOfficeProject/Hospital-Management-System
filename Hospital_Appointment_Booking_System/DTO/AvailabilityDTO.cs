namespace Hospital_Appointment_Booking_System.DTO
{
    public class AvailabilityDTO
    {
        public int AvailabilityId { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public UserDTO User { get; set; }
    }
}
