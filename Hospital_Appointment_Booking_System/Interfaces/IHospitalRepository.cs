using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IHospitalRepository
    {
        Task<IEnumerable<HospitalDTO>> GetAllHospital();
        Task<HospitalDTO> GetByIdHospital(int hospitalId);
        Task<bool> AddHospital(HospitalDTO hospitalDto);
        Task<bool> UpdateHospital(int hospitalId,HospitalDTO hospitalDto);
        Task DeleteHospital(int hospitalId);
    }
}