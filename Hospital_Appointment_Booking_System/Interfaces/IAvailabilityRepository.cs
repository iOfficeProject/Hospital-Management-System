using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IAvailabilityRepository
    {
        Task<Availability> GetById(int id);
        Task<List<Availability>> GetAll();
        Task Add(Availability availability);
        Task Update(Availability availability);
        Task Delete(Availability availability);
    }
}