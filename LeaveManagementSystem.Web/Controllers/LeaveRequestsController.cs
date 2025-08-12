using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypesService, ILeaveRequestsService _leaveRequestsService) : Controller
    {
        // Read - Allow employees to view their leave 
        public async Task<IActionResult> Index()
        {
            // Logic to get the leave requests for the currently logged in employee
            var model = await _leaveRequestsService.GetEmployeeLeaveRequests();

            return View(model);
        }

        // Create - Allow employees to create a new leave request
        public async Task<IActionResult> Create(int? leaveTypeId)
        {
            // Logic to create a leave request
            var leaveTypes = await _leaveTypesService.GetAll();
            var leaveTypesList = new SelectList(leaveTypes, "LeaveTypeId", "LeaveTypeName", leaveTypeId);
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList,
            };

            return View(model);
        }

        // Create - Allow employees to submit a leave request, i.e post request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model) // view model
        {
            // ensure tha the number of days does not exceed the maximum allowed for the leave type / what the employee has available 
            if (await _leaveRequestsService.DaysExceedAllocation(model))
            {
                ModelState.AddModelError(string.Empty, "You have exceeded your allocation.");
                ModelState.AddModelError(nameof(model.EndDate), "The number of days requested is invalid.");
            }

            if (ModelState.IsValid)
            {
                await _leaveRequestsService.CreateLeaveRequest(model);
                return RedirectToAction(nameof(Index));
            }

            var leaveTypes = await _leaveTypesService.GetAll();
            model.LeaveTypes = new SelectList(leaveTypes, "LeaveTypeId", "LeaveTypeName"); // must re-populate the select list after model validation - select list is not bound to the model

            return View(model);
        }

        // Update - Allow employees to cancel their leave requests
        [HttpPost] // post - implies there is a form submission
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            // Logic to cancel a leave request
            await _leaveRequestsService.CancelLeaveRequest(id);

            return RedirectToAction(nameof(Index));
        }

        // Allow admin/supervisor to view all leave requests
        [Authorize(Policy = "AdminSupervisorOnly")]
        public async Task<IActionResult> ListRequests()
        {
            // Logic to get all leave requests
            var model = await _leaveRequestsService.GetAllLeaveRequests();
            return View(model);
        }

        // Allow admin/supervisor to review leave requests
        public async Task<IActionResult> Review(int id)
        {
            // Logic to get a specific leave request for review
            var model = await _leaveRequestsService.GetLeaveRequestForReview(id);
            return View(model);
        }

        // Allow admin/supervisor to submit their review, i.e post request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, bool approved) 
        {
            await _leaveRequestsService.ReviewLeaveRequest(id, approved);

            return RedirectToAction(nameof(ListRequests));
        }
    }
}
