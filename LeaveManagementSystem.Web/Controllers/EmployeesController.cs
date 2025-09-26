using LeaveManagementSystem.Application.Models.Employees;
using LeaveManagementSystem.Application.Services.Employees;
using LeaveManagementSystem.Application.Services.LeaveTypes;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]

    public class EmployeesController(IEmployeesService _employeesService, ILeaveTypesService _leaveTypesService) : Controller
    {
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employees = await _employeesService.GetEmployees();
            return View(employees);
        }

        public async Task<IActionResult> Details(string? userId)
        {
            var employeeVM = await _employeesService.GetEmployeeAllocations(userId);
            return View(employeeVM);
        }
    }
}
