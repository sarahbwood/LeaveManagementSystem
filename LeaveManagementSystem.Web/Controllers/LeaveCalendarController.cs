using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveCalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
