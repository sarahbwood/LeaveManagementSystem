using LeaveManagementSystem.Application.Models.LeaveAllocations;

namespace LeaveManagementSystem.Application.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string EmployeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);
        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId);
        Task<List<EmployeeListVM>> GetEmployees();
        Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId);
        Task EditAllocation(LeaveAllocationEditVM leaveAllocationEditVM);
    }
}
