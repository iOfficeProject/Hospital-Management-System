using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Helpers;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Hospital_Appointment_Booking_System.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [Authorize]
    [EnableCors("MyPolicy")]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _IUserRepository;
        public UserController(IUserRepository iUserRepository)
        {
            _IUserRepository = iUserRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDto)
        {
            try
            {
                var user = new User
                {
                    Name = userDto.Name,
                    Email = userDto.Email,
                    Password = PasswordHasher.EncryptPassword(userDto.Password),
                    MobileNumber = userDto.MobileNumber,
                    RoleId = userDto.RoleId,
                    SpecializationId = userDto.SpecializationId,
                    HospitalId = userDto.HospitalId
                };

                bool userCreated = await _IUserRepository.AddUser(user);
                if (!userCreated)
                {
                    return Conflict("Email or mobile number already exists.");
                }

                return Ok(user);
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

                var userDto = new UserDTO
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    MobileNumber = (long)user.MobileNumber,
                    RoleId = user.RoleId,
                    SpecializationId = user.SpecializationId,
                    HospitalId = user.HospitalId
                };

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
                var existingUser = await _IUserRepository.GetUserById(id);
            try 
            { 

                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Name = updatedUserDto.Name;
                existingUser.Email = updatedUserDto.Email;
                existingUser.Password = PasswordHasher.EncryptPassword(updatedUserDto.Password);
                existingUser.MobileNumber = updatedUserDto.MobileNumber;
                existingUser.RoleId = updatedUserDto.RoleId;
                existingUser.SpecializationId = updatedUserDto.SpecializationId;
                existingUser.HospitalId = updatedUserDto.HospitalId;

            var updateResult = await _IUserRepository.UpdateUser(existingUser);
             if (!updateResult)
                {
                    return Conflict("Email or mobile number already exists.");
                }

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding user.");
            }
            }


            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                try
                {
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

            [HttpGet("User/{roleId}")]
            public async Task<ActionResult<List<UserDTO>>> GetUsersByRoleId(int roleId)
            {
            try
            {

                List<UserDTO> userDTOs = await _IUserRepository.GetUsersByRoleId(roleId);
                return Ok(userDTOs);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the user.");
            }
        }

        [HttpGet("specializedDoctors/{specializationId}")]
        public async Task<ActionResult<List<UserDTO>>> GetUsersBySpecializationId(int specializationId)
        {
            try
            {
                var users = await _IUserRepository.GetUsersBySpecializationId(specializationId);

                if (users == null || !users.Any())
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting specialized user.");
            }
        }
    }
}