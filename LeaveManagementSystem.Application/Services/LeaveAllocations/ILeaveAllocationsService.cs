using LeaveManagementSystem.Application.Models.LeaveAllocations;

namespace LeaveManagementSystem.Application.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);
        Task<List<EmployeeListVM>> GetEmployees();
    }
}
