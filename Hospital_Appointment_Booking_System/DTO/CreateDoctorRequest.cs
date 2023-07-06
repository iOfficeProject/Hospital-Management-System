using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.DTO
{
       
        public class CreateDoctorRequest
        {
        
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public long MobileNumber { get; set; }
        public int? RoleId { get; set; }
        public int? SpecializationId { get; set; }
        public int? HospitalId { get; set; }
        public int RoleID { get; set; }
       }   

}
