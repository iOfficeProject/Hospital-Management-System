using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IRoleRepository
    {
        Task AddRole(Role role);
        Task DeleteRole(int roleId);
        Task<List<Role>> GetAllRoles();
        Task<Role> GetRoleById(int roleId);

    }
}
