using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalRepository _hospitalRepository;

        public HospitalController(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetHospitals()
        {
            try
            {
                var hospitals = await _hospitalRepository.GetAllAsync();
                return Ok(hospitals);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving hospitals.");
            }
        }

        [HttpGet("{hospitalId}")]
        public async Task<ActionResult<Hospital>> GetHospital(int hospitalId)
        {
            try
            {
                var hospital = await _hospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return NotFound();
                }
                return Ok(hospital);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the hospital.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Hospital>> CreateHospital(Hospital hospital)
        {
            try
            {
                await _hospitalRepository.AddAsync(hospital);
                return CreatedAtAction(nameof(GetHospital), new { hospitalId = hospital.HospitalId }, hospital);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the hospital.");
            }
        }

        [HttpPut("{hospitalId}")]
        public async Task<ActionResult<Hospital>> UpdateHospital(int hospitalId, Hospital hospital)
        {
            try
            {
                await _hospitalRepository.UpdateAsync(hospitalId, hospital);
                var updatedHospital = await _hospitalRepository.GetByIdAsync(hospitalId);
                if (updatedHospital == null)
                {
                    return NotFound();
                }
                return Ok(updatedHospital);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the hospital.");
            }
        }

        [HttpDelete("{hospitalId}")]
        public async Task<IActionResult> DeleteHospital(int hospitalId)
        {
            try
            {
                await _hospitalRepository.DeleteAsync(hospitalId);
                return Ok("Hospital is deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the hospital.");
            }
        }
    }
}
