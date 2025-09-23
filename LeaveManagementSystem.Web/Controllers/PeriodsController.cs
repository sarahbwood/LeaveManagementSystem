using LeaveManagementSystem.Application.Models.Periods;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class PeriodsController() : Controller
    {
        // GET: Periods
        public async Task<IActionResult> Index()
        {
            return View();
        }

       
  
       
    }
}
