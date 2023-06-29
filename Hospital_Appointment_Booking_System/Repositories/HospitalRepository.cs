using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
        public class HospitalRepository : IHospitalRepository
        {
            readonly Master_Hospital_ManagementContext _dbContext = new();


            public HospitalRepository(Master_Hospital_ManagementContext dbContext)
            {
                _dbContext = dbContext;

            }

        public async Task AddUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(); 
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _dbContext.Set<User>().ToListAsync();
        }
    }
 }

