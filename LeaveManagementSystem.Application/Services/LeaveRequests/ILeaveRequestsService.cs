namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests();
        Task<EmployeeLeaveRequestListVM> GetAllLeaveRequests();
        Task CancelLeaveRequest(int leaveRequestId);
        Task<LeaveRequestReviewVM> GetLeaveRequestForReview(int leaveRequestId);
        Task ReviewLeaveRequest(int leaveRequestId, bool isApproved);
        Task<bool> DaysExceedAllocation(LeaveRequestCreateVM model);

    }
}