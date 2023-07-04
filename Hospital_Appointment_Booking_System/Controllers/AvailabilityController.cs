using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    public class AvailabilityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
