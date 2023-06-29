using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IHospitalRepository
    {
        Task<List<User>> GetAllUser();
    }
}
