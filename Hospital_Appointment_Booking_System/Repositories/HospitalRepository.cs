using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly Master_Hospital_ManagementContext _context;

        public HospitalRepository(Master_Hospital_ManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hospital>> GetAllAsync()
        {
            return await _context.Hospitals.ToListAsync();
        }

        public async Task<Hospital> GetByIdAsync(int hospitalId)
        {
            return await _context.Hospitals.FindAsync(hospitalId);
        }

        public async Task AddAsync(Hospital hospital)
        {
            await _context.Hospitals.AddAsync(hospital);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int hospitalId, Hospital hospital)
        {
            var existingHospital = await _context.Hospitals.FindAsync(hospitalId);
            if (existingHospital != null)
            {
                existingHospital.HospitalName = hospital.HospitalName;
                existingHospital.Location = hospital.Location;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int hospitalId)
        {
            var hospital = await _context.Hospitals.FindAsync(hospitalId);
            if (hospital != null)
            {
                _context.Hospitals.Remove(hospital);
                await _context.SaveChangesAsync();
            }
        }
    }
}
