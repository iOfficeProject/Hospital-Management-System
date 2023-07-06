using Hospital_Appointment_Booking_System.DTO;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<AppointmentDTO>> GetAllAppointments();
        Task<AppointmentDTO> GetAppointmentById(int id);
        Task AddAppointment(AppointmentInputDTO appointmentDto);
        Task UpdateAppointment(AppointmentInputDTO appointmentDto);
        Task DeleteAppointment(int id);
    }
}
