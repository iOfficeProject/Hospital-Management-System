using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        readonly Master_Hospital_ManagementContext _dbContext = new();
        private readonly IMapper _mapper;

        public AppointmentRepository(Master_Hospital_ManagementContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAllAppointments()
        {
            var appointments = await _dbContext.Appointments.ToListAsync();
            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
        }

        public async Task<AppointmentDTO> GetAppointmentById(int id)
        {
            var appointment = await _dbContext.Appointments.FindAsync(id);
            return _mapper.Map<AppointmentDTO>(appointment);
        }

        public async Task AddAppointment(AppointmentInputDTO appointmentDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentDto);
            _dbContext.Appointments.Add(appointment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAppointment(AppointmentInputDTO appointmentDto)
        {
            var appointment = await _dbContext.Appointments.FindAsync(appointmentDto.AppointmentId);
            if (appointment == null)
                throw new ArgumentException("Invalid appointment id");

            _mapper.Map(appointmentDto, appointment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAppointment(int id)
        {
            var appointment = await _dbContext.Appointments.FindAsync(id);
            if (appointment == null)
                throw new ArgumentException("Invalid appointment id");

            _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync();
        }

    }
}
