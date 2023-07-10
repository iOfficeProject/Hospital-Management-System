using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        readonly Master_Hospital_ManagementContext _dbContext = new();
        private readonly IMapper _mapper;

        public SpecializationRepository(Master_Hospital_ManagementContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SpecializationDTO>> GetAllSpecializations()
        {
            var specializations = await _dbContext.Specializations.Include(s => s.Hospital).ToListAsync();
            return _mapper.Map<IEnumerable<SpecializationDTO>>(specializations);
        }

        public async Task<SpecializationDTO> GetSpecializationById(int id)
        {
            var specialization = await _dbContext.Specializations.Include(s => s.Hospital)
                .FirstOrDefaultAsync(s => s.SpecializationId == id);
            return _mapper.Map<SpecializationDTO>(specialization);
        }

        public async Task<bool> AddSpecialization(SpecializationDTO specializationDto)
        {
            var existingSpecialization = await _dbContext.Specializations.FirstOrDefaultAsync(s => s.SpecializationName == specializationDto.SpecializationName);

            if (existingSpecialization != null)
            {
                return false;
            }

            var specialization = _mapper.Map<Specialization>(specializationDto);
            _dbContext.Specializations.Add(specialization);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task UpdateSpecialization(SpecializationDTO specializationDto)
        {
            var specialization = await _dbContext.Specializations.FindAsync(specializationDto.SpecializationId);
            if (specialization == null)
                throw new ArgumentException("Invalid specialization id");

            _mapper.Map(specializationDto, specialization);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSpecialization(int id)
        {
            var specialization = await _dbContext.Specializations.FindAsync(id);
            if (specialization == null)
                throw new ArgumentException("Invalid specialization id");

            _dbContext.Specializations.Remove(specialization);
            await _dbContext.SaveChangesAsync();
        }
    }
}

