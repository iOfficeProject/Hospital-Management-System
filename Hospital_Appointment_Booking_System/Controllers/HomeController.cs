using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class HomeController:ControllerBase
    {
        private readonly IHospitalRepository _IHospitalRepository;
        private readonly IMapper _mapper;
        public HomeController(IHospitalRepository iHospitalRepository)
        {
            _IHospitalRepository = iHospitalRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _IHospitalRepository.GetAllUser();
            if (users != null)
            {
                var records = users.Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    MobileNumber = u.MobileNumber,
                    RoleId = u.RoleId,
                    SpecializationId = u.SpecializationId,
                    HospitalId = u.HospitalId
                }).ToList();

                return Ok(records);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
