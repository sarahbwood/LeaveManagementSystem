using LeaveManagementSystem.Application.Models.Departments;
using LeaveManagementSystem.Application.Models.LeaveAllocations;

namespace LeaveManagementSystem.Application.Services.Departments
{
    public interface IDepartmentsService
    {
        Task<DepartmentsReadOnlyListVM> GetAllDepartments();
        Task<List<EmployeeListVM>> GetManagers();
        Task CreateDepartment(DepartmentCreateVM departmentCreateVM);
        
    }
}