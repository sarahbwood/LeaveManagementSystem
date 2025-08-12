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

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? id)
        {
            await _leaveAllocationsService.AllocateLeave(id);
            return RedirectToAction(nameof(Details), new { userId = id });
        }

        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveAllocation = await _leaveAllocationsService.GetEmployeeAllocation(id.Value);
            if (leaveAllocation == null)
            {
                return NotFound();
            }

            return View(leaveAllocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM leaveAllocationEditVM)
        {
            if (await _leaveTypesService.DaysExceedMaximum(leaveAllocationEditVM.LeaveType.LeaveTypeId, leaveAllocationEditVM.NumberOfDays))
            {
                ModelState.AddModelError(nameof(leaveAllocationEditVM.NumberOfDays), "Number of days exceeds maximum allowed for this leave type.");
            }

            if (ModelState.IsValid)
            {
                await _leaveAllocationsService.EditAllocation(leaveAllocationEditVM);
                return RedirectToAction(nameof(Details), new { userId = leaveAllocationEditVM.Employee.Id });


            }

            var days = leaveAllocationEditVM.NumberOfDays;
            leaveAllocationEditVM = await _leaveAllocationsService.GetEmployeeAllocation(leaveAllocationEditVM.Id); // Re-fetch the allocation to ensure that the view model isn't missing any data
            leaveAllocationEditVM.NumberOfDays = days; // Restore user selected number of days to the view model

            return View(leaveAllocationEditVM);
        }
    }
}
