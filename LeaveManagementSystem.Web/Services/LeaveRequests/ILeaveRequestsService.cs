using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests();
        Task<EmployeeLeaveRequestListVM> GetAllLeaveRequests();
        Task CancelLeaveRequest(int leaveRequestId);
        Task ReviewLeaveRequest(LeaveRequestReviewVM model);
        Task<bool> DaysExceedAllocation(LeaveRequestCreateVM model);

    }
}