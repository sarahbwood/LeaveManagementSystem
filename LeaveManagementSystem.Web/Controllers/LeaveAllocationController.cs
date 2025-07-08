using System.Threading.Tasks;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]

    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employees = await _leaveAllocationsService.GetEmployees();
            return View(employees);
        }

        public async Task<IActionResult> Details(string? userId)
        {
            var employeeVM = await _leaveAllocationsService.GetEmployeeAllocations(userId);
            return View(employeeVM);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? id)
        {
            await _leaveAllocationsService.AllocateLeave(id);
            return RedirectToAction(nameof(Details), new {userId = id});
        }
    }
}
