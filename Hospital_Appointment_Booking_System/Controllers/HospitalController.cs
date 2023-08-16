using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public HospitalController(IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetHospitals()
        {
            try
            {
                var hospitals = await _hospitalRepository.GetAllHospital();
                return Ok(hospitals);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving hospitals.");
            }
        }

        [HttpGet("{hospitalId}")]
        public async Task<IActionResult> GetHospital(int hospitalId)
        {
            try
            {
                var hospital = await _hospitalRepository.GetByIdHospital(hospitalId);
                if (hospital == null)
                {
                    return NotFound();
                }
                return Ok(hospital);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the hospital.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateHospital(HospitalDTO hospitalDto)
        {
            try
            {
                bool hospitalCreated = await _hospitalRepository.AddHospital(hospitalDto);
                if (!hospitalCreated)
                {
                    return Conflict("Hospital name already exists.");
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the hospital.");
            }
        }

        [HttpPut("{hospitalId}")]
        public async Task<ActionResult<HospitalDTO>> UpdateHospital(int hospitalId, HospitalDTO hospitalDto)
        {
            try
            {
                var isHospitalUpdated = await _hospitalRepository.UpdateHospital(hospitalId, hospitalDto);
                if (!isHospitalUpdated)
                {
                    return Conflict("Hospital name already exists.");
                }
                var updatedHospital = await _hospitalRepository.GetByIdHospital(hospitalId);
                if (updatedHospital == null)
                {
                    return NotFound();
                }
                return Ok(updatedHospital);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the hospital.");
            }
        }

        [HttpDelete("{hospitalId}")]
        public async Task<IActionResult> DeleteHospital(int hospitalId)
        {
            try
            {
                await _hospitalRepository.DeleteHospital(hospitalId);
                return Ok("Hospital is deleted.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the hospital.");
            }
        }
    }
}