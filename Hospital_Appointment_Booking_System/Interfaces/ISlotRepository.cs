using Hospital_Appointment_Booking_System.DTO;

namespace Hospital_Appointment_Booking_System.Interfaces
{
    public interface ISlotRepository
    {
        Task<IEnumerable<SlotDTO>> GetAllSlots();
        Task<SlotDTO> GetSlotById(int id);
        Task AddSlot(SlotDTO slotDto);
        Task UpdateSlot(SlotDTO slotDto);
        Task DeleteSlot(int id);
    }
}
