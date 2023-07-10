using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;




namespace Hospital_Appointment_Booking_System.Controllers
{
    // [Authorize]
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

        [HttpGet("GetUserList")]
        public async Task<ActionResult<List<User>>> GetUsers(RoleDTO roledto)
        {
            try
            {
                List<User> users = await _IUserRepository.GetAllUser(roledto);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving Users.");
            }
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
                    Password = userDto.Password,
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


        [HttpGet("GetDoctorList")]
        [Authorize]
        public async Task<ActionResult<List<User>>> GetDoctors(RoleDTO roledto)
        {
            try
            {
                List<User> doctors = await _IUserRepository.GetDoctors(roledto);
                return Ok(doctors);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving Doctors.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO updatedUserDto)
        {
            var existingUser = await _IUserRepository.GetUserById(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Name = updatedUserDto.Name;
            existingUser.Email = updatedUserDto.Email;
            existingUser.Password = updatedUserDto.Password;
            existingUser.MobileNumber = updatedUserDto.MobileNumber;
            existingUser.RoleId = updatedUserDto.RoleId;
            existingUser.SpecializationId = updatedUserDto.SpecializationId;
            existingUser.HospitalId = updatedUserDto.HospitalId;

            try
            {
                await _IUserRepository.UpdateUser(existingUser);
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
                await _IUserRepository.DeleteUser(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
            }
        }
    }
}