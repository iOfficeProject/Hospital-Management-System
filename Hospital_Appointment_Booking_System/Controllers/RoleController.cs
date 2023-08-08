using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [Authorize]
    [EnableCors("MyPolicy")]
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _IRoleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository iRoleRepository, IMapper mapper)
        {
            _IRoleRepository = iRoleRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDTO roleDTO)
        {
            try
            {
                var role = _mapper.Map<Role>(roleDTO);
                bool roleCreated = await _IRoleRepository.AddRole(role);
                if (!roleCreated)
                {
                    return Conflict("Role already exists.");
                }

                return Ok(roleDTO);
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
                var user = await _IRoleRepository.GetRoleById(roleId);
                if (user == null)
                {
                    return NotFound();
                }
                await _IRoleRepository.DeleteRole(roleId);
                return Ok(roleId);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleDTO>>> GetAllRoles()
        {
            try
            {
                var roles = await _IRoleRepository.GetAllRoles();
                if (roles != null)
                {
                    var roleDTOs = _mapper.Map<List<RoleDTO>>(roles);
                    return Ok(roleDTOs);
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

                var roleDTO = _mapper.Map<RoleDTO>(role);
                return Ok(roleDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the user.");
            }
        }
    }
}
