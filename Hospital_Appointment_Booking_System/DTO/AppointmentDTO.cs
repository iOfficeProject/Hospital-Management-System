namespace Hospital_Appointment_Booking_System.DTO
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public DateTime? AppointmentStartTime { get; set; }
        public DateTime? AppointmentEndTime { get; set; }
        public SlotDTO Slot { get; set; }
        public HospitalDTO Hospital { get; set; }
        public UserDTO User { get; set; }
    }
}
