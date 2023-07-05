using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [ApiController]
    [Route("api/specializations")]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationRepository _specializationRepository;

        public SpecializationController(ISpecializationRepository specializationRepository)
        {
            _specializationRepository = specializationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecializations()
        {
            var specializations = await _specializationRepository.GetAllSpecializations();
            return Ok(specializations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecializationById(int id)
        {
            var specialization = await _specializationRepository.GetSpecializationById(id);
            if (specialization == null)
                return NotFound();

            return Ok(specialization);
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialization(SpecializationDTO specializationDto)
        {
            await _specializationRepository.AddSpecialization(specializationDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialization(int id, SpecializationDTO specializationDto)
        {
            specializationDto.SpecializationId = id;
            await _specializationRepository.UpdateSpecialization(specializationDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            await _specializationRepository.DeleteSpecialization(id);
            return Ok();
        }
    }
}
