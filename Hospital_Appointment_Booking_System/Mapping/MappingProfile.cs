using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Models;

namespace Hospital_Appointment_Booking_System.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<SpecializationDTO, Specialization>().ReverseMap();
            CreateMap<SlotDTO, Slot>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<PaymentDTO, Payment>().ReverseMap();
            CreateMap<HospitalDTO, Hospital>().ReverseMap();
            CreateMap<AvailabilityDTO, Availability>().ReverseMap();
            CreateMap<AppointmentDTO, Appointment>().ReverseMap();
        }

    }
}
