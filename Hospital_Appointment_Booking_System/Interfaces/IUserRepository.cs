using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUser();
        Task AddUser(User user);
        Task<List<User>> GetDoctors(RoleDTO roledto);
        Task UpdateUser(User user);
        Task DeleteUser(int userId);
        Task<User> GetUserById(int userId);

    }
}