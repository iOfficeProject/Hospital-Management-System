using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController:ControllerBase
    {
        private readonly IHospitalRepository _IHospitalRepository;
        private readonly IMapper _mapper;
        public UserController(IHospitalRepository iHospitalRepository)
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
                    Password = u.Password,
                    MobileNumber = (long)u.MobileNumber,
                    RoleId = u.RoleId,
                    SpecializationId = u.SpecializationId,
                    HospitalId = u.HospitalId
                }).ToList();
                /*var records = await _IHospitalRepository.GetAllUser();*/
                return Ok(records);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password=userDto.Password,
                MobileNumber = userDto.MobileNumber,
                RoleId = userDto.RoleId,
                SpecializationId = userDto.SpecializationId,
                HospitalId = userDto.HospitalId
            };

            await _IHospitalRepository.AddUser(user);

            return Ok(user);
        }
    }
}
