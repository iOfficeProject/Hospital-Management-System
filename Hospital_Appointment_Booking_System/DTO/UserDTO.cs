namespace Hospital_Appointment_Booking_System.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long MobileNumber { get; set; }
        public RoleDTO Role { get; set; }
        public SpecializationDTO Specialization { get; set; }
        public HospitalDTO Hospital { get; set; }
    }
}
