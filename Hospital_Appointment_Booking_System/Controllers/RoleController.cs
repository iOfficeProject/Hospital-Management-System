using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [Authorize]
    [EnableCors("MyPolicy")]
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _IRoleRepository;
        public RoleController(IRoleRepository iRoleRepository)
        {
            _IRoleRepository = iRoleRepository;
        }


        [HttpPost]
        public async Task<ActionResult<Role>> AddRole(Role role)
        {
            try
            {

                bool roleCreated = await _IRoleRepository.AddRole(role);
                if (!roleCreated)
                {
                    return Conflict("Role already exists.");
                }

                return Ok(role);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the role.");
            }
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            try
            {
                await _IRoleRepository.DeleteRole(roleId);
                return Ok(roleId);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Role>>> GetAllRoles()
        {
            try
            {
                var roles = await _IRoleRepository.GetAllRoles();
                if (roles != null)
                {
                    var records = roles.Select(u => new RoleDTO
                    {
                        RoleId = u.RoleId,
                        RoleName = u.RoleName

                    }).ToList();

                    return Ok(records);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting users.");
            }
        }


        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            try
            {
                var role = await _IRoleRepository.GetRoleById(roleId);

                if (role == null)
                {
                    return NotFound("Role not found");
                }

                return Ok(role);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the user.");
            }
        }

    }
}
