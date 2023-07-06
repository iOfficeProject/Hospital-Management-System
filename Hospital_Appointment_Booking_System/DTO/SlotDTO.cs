namespace Hospital_Appointment_Booking_System.DTO
{
    public class SlotDTO
    {
        public int SlotId { get; set; }
        public DateTime? SlotDate { get; set; }
        public DateTime? SlotStartTime { get; set; }
        public DateTime? SlotEndTime { get; set; }
        public AvailabilityDTO Availability { get; set; }
        public HospitalDTO  Hospital { get; set; }
        public UserDTO User { get; set; }
    }
}
