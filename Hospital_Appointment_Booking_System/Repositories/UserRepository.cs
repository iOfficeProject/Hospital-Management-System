using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly Master_Hospital_ManagementContext _dbContext = new();


        public UserRepository(Master_Hospital_ManagementContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task AddUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddDoctor(User user)
        {
            // Save user and role to the database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _dbContext.Set<User>().ToListAsync();
        }

        public async Task<List<User>> GetDoctors(RoleDTO role)
        {
            List<User> doctors = await _dbContext.Users
         .Where(u => u.Role.RoleName == "Doctor")
        .ToListAsync();

            return doctors;
        }
    }
}

