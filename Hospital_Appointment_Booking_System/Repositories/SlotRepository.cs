using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Appointment_Booking_System.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        readonly Master_Hospital_ManagementContext _dbContext = new();
        private readonly IMapper _mapper;

        public SlotRepository(Master_Hospital_ManagementContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SlotDTO>> GetAllSlots()
        {
            var slots = await _dbContext.Slots.ToListAsync();
            return _mapper.Map<IEnumerable<SlotDTO>>(slots);
        }

        public async Task<SlotDTO> GetSlotById(int id)
        {
            var slot = await _dbContext.Slots.FindAsync(id);
            return _mapper.Map<SlotDTO>(slot);
        }

        public async Task AddSlot(SlotDTO slotDto)
        {
            var slot = _mapper.Map<Slot>(slotDto);
            _dbContext.Slots.Add(slot);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSlot(SlotDTO slotDto)
        {
            var slot = await _dbContext.Slots.FindAsync(slotDto.SlotId);
            if (slot == null)
                throw new ArgumentException("Invalid slot id");

            _mapper.Map(slotDto, slot);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSlot(int id)
        {
            var slot = await _dbContext.Slots.FindAsync(id);
            if (slot == null)
                throw new ArgumentException("Invalid slot id");

            _dbContext.Slots.Remove(slot);
            await _dbContext.SaveChangesAsync();
        }
    }
}
