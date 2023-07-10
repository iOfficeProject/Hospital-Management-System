using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IHospitalRepository
    {       
        Task<IEnumerable<Hospital>> GetAllHospital();
        Task<Hospital> GetByIdHospital (int hospitalId);
        Task<bool> AddHospital(Hospital hospital);
        Task UpdateHospital(int hospitalId, Hospital hospital);
        Task DeleteHospital(int hospitalId);
    }
}