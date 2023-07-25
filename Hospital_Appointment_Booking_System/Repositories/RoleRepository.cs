using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        readonly Master_Hospital_ManagementContext _dbContext = new();

        public RoleRepository(Master_Hospital_ManagementContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<bool> AddRole(Role role)
        {
            var existingRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == role.RoleName);
            if (existingRole != null)
            {
                return false;
            }
            _dbContext.Roles.Add(role);
            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task DeleteRole(int roleId)
        {
            var role = await _dbContext.Roles.FindAsync(roleId);

            if (role != null)
            {
                _dbContext.Roles.Remove(role);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<Role> GetRoleById(int roleId)
        {
            return await _dbContext.Roles.FindAsync(roleId);
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await _dbContext.Set<Role>().ToListAsync();
        }
    }
}
