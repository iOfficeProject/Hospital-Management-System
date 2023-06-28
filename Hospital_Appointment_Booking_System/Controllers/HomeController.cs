using Microsoft.AspNetCore.Mvc;

namespace Hospital_Appointment_Booking_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
