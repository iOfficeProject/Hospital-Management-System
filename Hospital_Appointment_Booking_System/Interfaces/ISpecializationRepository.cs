using Hospital_Appointment_Booking_System.DTO;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<IEnumerable<SpecializationDTO>> GetAllSpecializations();
        Task<SpecializationDTO> GetSpecializationById(int id);
        Task<bool> AddSpecialization(SpecializationDTO specializationDto);
        Task UpdateSpecialization(SpecializationDTO specializationDto);
        Task DeleteSpecialization(int id);
        Task<IEnumerable<SpecializationDTO>> GetSpecializationsByHospitalId(int hospitalId);
    }
}
