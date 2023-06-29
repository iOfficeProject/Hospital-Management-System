namespace Hospital_Appointment_Booking_System.DTO
{
    public class SpecializationDTO
    {
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public HospitalDTO Hospital { get; set; }
    }
}
