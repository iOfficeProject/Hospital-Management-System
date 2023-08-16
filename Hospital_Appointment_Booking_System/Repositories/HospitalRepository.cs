using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly Master_Hospital_ManagementContext _context;
        private readonly IMapper _mapper;


        public HospitalRepository(Master_Hospital_ManagementContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HospitalDTO>> GetAllHospital()
        {
            var hospitals = await _context.Hospitals.ToListAsync();
            return _mapper.Map<IEnumerable<HospitalDTO>>(hospitals);
        }

        public async Task<HospitalDTO> GetByIdHospital(int hospitalId)
        {

            var hospital = await _context.Hospitals.FirstOrDefaultAsync(s => s.HospitalId == hospitalId);
            return _mapper.Map<HospitalDTO>(hospital);

        }

        public async Task<bool> AddHospital(HospitalDTO hospitalDto)
        {
            var hospital = _mapper.Map<Hospital>(hospitalDto);
            var existingHospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.HospitalName == hospital.HospitalName && h.Location == hospital.Location);
            if (existingHospital != null)
            {
                return false; 
            }
            _context.Hospitals.Add(hospital);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHospital(int hospitalId, HospitalDTO hospitalDto)
        {
            var existingHospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == hospitalId);
            if (existingHospital == null)
            {
                return false;
            }
            var anotherHospitalWithSameName = await _context.Hospitals.FirstOrDefaultAsync(h => h.HospitalName == hospitalDto.HospitalName && h.Location == hospitalDto.Location && h.HospitalId != hospitalId);
            if (anotherHospitalWithSameName != null)
            {
                return false;
            }

            existingHospital.HospitalName = hospitalDto.HospitalName;
            existingHospital.Location = hospitalDto.Location;

            await _context.SaveChangesAsync();
            _mapper.Map<HospitalDTO>(existingHospital);
            return true;
        }

        public async Task DeleteHospital(int hospitalId)
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