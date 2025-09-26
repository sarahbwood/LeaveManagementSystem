using LeaveManagementSystem.Application.Models.Employees;

namespace LeaveManagementSystem.Application.Services.Employees
{
    public interface IEmployeesService
    {
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);
        Task<List<EmployeeListVM>> GetEmployees();
    }
}
