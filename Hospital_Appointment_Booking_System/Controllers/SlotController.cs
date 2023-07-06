using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    [ApiController]
    [Route("api/Slots")]
    public class SlotController : Controller
    {
        private readonly ISlotRepository _slotRepository;

        public SlotController(ISlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSlots()
        {
            var slots = await _slotRepository.GetAllSlots();
            return Ok(slots);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSlotById(int id)
        {
            var slot = await _slotRepository.GetSlotById(id);
            if (slot == null)
                return NotFound();

            return Ok(slot);
        }

        [HttpPost]
        public async Task<IActionResult> AddSlot(SlotDTO slotDto)
        {
            await _slotRepository.AddSlot(slotDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlot(int id, SlotDTO slotDto)
        {
            slotDto.SlotId = id;
            await _slotRepository.UpdateSlot(slotDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlot(int id)
        {
            await _slotRepository.DeleteSlot(id);
            return Ok();
        }
    }
}
