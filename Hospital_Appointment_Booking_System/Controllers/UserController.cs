using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Helpers;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Hospital_Appointment_Booking_System.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [Authorize]
    [EnableCors("MyPolicy")]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _IUserRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository iUserRepository, IMapper mapper)
        {
            _IUserRepository = iUserRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.Password = PasswordHasher.EncryptPassword(userDto.Password);

                bool userCreated = await _IUserRepository.AddUser(user);
                if (!userCreated)
                {
                    return Conflict("Email or mobile number already exists.");
                }

                var createdUserDto = _mapper.Map<UserDTO>(user);
                return Ok(createdUserDto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding users.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            try
            {
                var user = await _IUserRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                var userDto = _mapper.Map<UserDTO>(user);
                return Ok(userDto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO updatedUserDto)
        {
            try
            {
                var existingUser = await _IUserRepository.GetUserById(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                var updatedUser = _mapper.Map<User>(updatedUserDto);
                updatedUser.UserId = id;
                updatedUser.Password = PasswordHasher.EncryptPassword(updatedUserDto.Password);

                bool userUpdated = await _IUserRepository.UpdateUser(updatedUser);
                if (!userUpdated)
                {
                    return Conflict("Email or mobile number already exists.");
                }

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _IUserRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }

                await _IUserRepository.DeleteUser(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            try
            {
                List<UserDTO> userDTOs = await _IUserRepository.GetAllUsers();
                return Ok(userDTOs);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting users.");
            }
        }
    }
}
