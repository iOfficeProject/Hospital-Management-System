using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IHospitalRepository
    {
        Task<IEnumerable<Hospital>> GetAllAsync();
        Task<Hospital> GetByIdAsync(int hospitalId);
        Task AddAsync(Hospital hospital);
        Task UpdateAsync(int hospitalId, Hospital hospital);
        Task DeleteAsync(int hospitalId);
    }
}
