using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var model = new TestViewModel
            {
                Name = "Bob",
                DateOfBirth = new DateTime(1972, 09, 22)
            };

            return View(model);
        }
    }
}
