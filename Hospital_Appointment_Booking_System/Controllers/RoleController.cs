using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
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
        public async Task<IActionResult> AddRole(RoleDTO roledto)
        {
            var role = new Role
            {
                RoleName = roledto.RoleName,           
            };

            await _IRoleRepository.AddRole(role);

            return Ok(role);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _IRoleRepository.DeleteRole(roleId);
            return Ok(roleId);
        }

        [HttpGet]
        public async Task<ActionResult<List<Role>>> GetAllUsers()
        {
            var roles = await _IRoleRepository.GetAllRoles();
            if (roles != null)
            {
                var records = roles.Select(u => new RoleDTO
                {
                    RoleId = u.RoleId,
                    RoleName = u.RoleName
                  
                }).ToList();
                /*var records = await _IHospitalRepository.GetAllUser();*/
                return Ok(records);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetUserById(int roleId)
        {
            var role = await _IRoleRepository.GetRoleById(roleId);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            return Ok(role);
        }

    }
}
