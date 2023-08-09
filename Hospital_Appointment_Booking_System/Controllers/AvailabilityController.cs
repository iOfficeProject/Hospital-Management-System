using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/availability")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IMapper _mapper;

        public AvailabilityController(IAvailabilityRepository availabilityRepository, IMapper mapper)
        {
            _availabilityRepository = availabilityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailability()
        {
            try
            {
                var availabilities = await _availabilityRepository.GetAllAvailability();
                return Ok(availabilities);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching availability data.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAvailabilityById(int id)
        {
            try
            {
                var availability = await _availabilityRepository.GetAvailabilityById(id);
                if (availability == null)
                {
                    return NotFound();
                }

                return Ok(availability);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching availability data by ID.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailability(AvailabilityDTO availabilityDTO)
        {
            try
            {
                var availability = _mapper.Map<Availability>(availabilityDTO);

                await _availabilityRepository.AddAvailability(availability);

                var mappedAvailabilityDTO = _mapper.Map<AvailabilityDTO>(availability);
                return Ok(mappedAvailabilityDTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding availability data.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, AvailabilityDTO availabilityDTO)
        {
            try
            {
                var availability = await _availabilityRepository.GetAvailabilityById(id);
                if (availability == null)
                {
                    return NotFound();
                }

                _mapper.Map(availabilityDTO, availability);

                await _availabilityRepository.UpdateAvailability(availability);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating availability data.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            try
            {
                var availability = await _availabilityRepository.GetAvailabilityById(id);
                if (availability == null)
                {
                    return NotFound();
                }

                await _availabilityRepository.DeleteAvailability(availability);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting availability data.");
            }
        }
    }
}
