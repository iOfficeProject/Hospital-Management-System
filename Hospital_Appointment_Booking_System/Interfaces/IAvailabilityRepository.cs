using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IAvailabilityRepository
    {
        Task<Availability> GetAvailabilityById(int id);
        Task<List<Availability>> GetAllAvailability();
        Task AddAvailability(Availability availability);
        Task UpdateAvailability(Availability availability);
        Task DeleteAvailability(Availability availability);
    }
}