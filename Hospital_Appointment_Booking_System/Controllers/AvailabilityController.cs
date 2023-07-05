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

        // GET: api/availability
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var availabilities = await _availabilityRepository.GetAll();
            return Ok(availabilities);
        }

        // GET: api/availability/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var availability = await _availabilityRepository.GetById(id);
            if (availability == null)
            {
                return NotFound();
            }

            return Ok(availability);
        }

        // POST: api/availability
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

            await _availabilityRepository.Add(availability);

            return Ok(availability);
        }

        // PUT: api/availability/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AvailabilityDTO availabilityDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var availability = await _availabilityRepository.GetById(id);
            if (availability == null)
            {
                return NotFound();
            }

            availability.IsAvailable = availabilityDTO.IsAvailable;
            availability.Date = availabilityDTO.Date;
            availability.StartTime = availabilityDTO.StartTime;
            availability.EndTime = availabilityDTO.EndTime;
            availability.UserId = availabilityDTO.UserId;

            await _availabilityRepository.Update(availability);

            return NoContent();
        }

        // DELETE: api/availability/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var availability = await _availabilityRepository.GetById(id);
            if (availability == null)
            {
                return NotFound();
            }

            await _availabilityRepository.Delete(availability);

            return NoContent();
        }
    }

}