using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/availability")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityRepository _availabilityRepository;

        public AvailabilityController(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailability()
        {
            var availabilities = await _availabilityRepository.GetAllAvailability();
            return Ok(availabilities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAvailabilityById(int id)
        {
            var availability = await _availabilityRepository.GetAvailabilityById(id);
            if (availability == null)
            {
                return NotFound();
            }

            return Ok(availability);
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailability(AvailabilityDTO availabilityDTO)
        {

            var availability = new Availability
            {
                IsAvailable = availabilityDTO.IsAvailable,
                Date = availabilityDTO.Date,
                StartTime = availabilityDTO.StartTime,
                EndTime = availabilityDTO.EndTime,
                UserId = availabilityDTO.UserId
            };

            await _availabilityRepository.AddAvailability(availability);

            return Ok(availability);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] AvailabilityDTO availabilityDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var availability = await _availabilityRepository.GetAvailabilityById(id);
            if (availability == null)
            {
                return NotFound();
            }

            availability.IsAvailable = availabilityDTO.IsAvailable;
            availability.Date = availabilityDTO.Date;
            availability.StartTime = availabilityDTO.StartTime;
            availability.EndTime = availabilityDTO.EndTime;
            availability.UserId = availabilityDTO.UserId;

            await _availabilityRepository.UpdateAvailability(availability);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            var availability = await _availabilityRepository.GetAvailabilityById(id);
            if (availability == null)
            {
                return NotFound();
            }

            await _availabilityRepository.DeleteAvailability(availability);

            return NoContent();
        }
    }

}