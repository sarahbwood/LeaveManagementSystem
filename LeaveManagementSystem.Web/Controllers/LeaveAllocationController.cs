using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveTypes;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]

    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypesService) : Controller
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
    }
}
