using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IHospitalRepository
    {
        Task<int> CreateUser(MasterUser masterUser);
        Task<IEnumerable<MasterUser>> GetUser();
        Task<MasterUser> GetUserById(int id);

    }
}
