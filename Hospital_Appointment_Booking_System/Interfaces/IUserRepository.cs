using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUser(User user);        
        Task<bool> UpdateUser(User user);
        Task DeleteUser(int userId);
        Task<User> GetUserById(int userId);
        Task<List<UserDTO>> GetAllUsers();
        Task<List<UserDTO>> GetUsersByRoleId(int roleId);
        Task<List<UserDTO>> GetUsersBySpecializationId(int specializationId);
    }
}