using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly string _ConnectionString;
        private readonly string _context;

        public HospitalRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
    
        }

        public async Task<int> CreateAsset(MasterUser masterUser)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateUser(MasterUser masterUser)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MasterUser>> GetUser()
        {
            throw new NotImplementedException();
        }

        public Task<MasterUser> GetUserById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
