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

        public async Task<bool> AddUser(User user)
        {
            var existingUserWithEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUserWithEmail != null)
            {
                return false; // Return false to indicate duplicate email
            }

            // Check for existing mobile number
            var existingUserWithMobileNumber = await _dbContext.Users.FirstOrDefaultAsync(u => u.MobileNumber == user.MobileNumber);
            if (existingUserWithMobileNumber != null)
            {
                return false; // Return false to indicate duplicate mobile number
            }

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUser(RoleDTO role)
        {
            List<User> users = await _dbContext.Users
         .Where(u => u.Role.RoleName == "User")
        .ToListAsync();
            return users;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task UpdateUser(User updatedUser)
        {
            var user = await _dbContext.Users.FindAsync(updatedUser.UserId);

            if (user != null)
            {
                // Update the relevant properties of the user
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                user.Password = updatedUser.Password;
                user.MobileNumber = updatedUser.MobileNumber;
                user.RoleId = updatedUser.RoleId;
                user.SpecializationId = updatedUser.SpecializationId;
                user.HospitalId = updatedUser.HospitalId;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
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