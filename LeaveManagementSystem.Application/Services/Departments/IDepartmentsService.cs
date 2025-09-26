using LeaveManagementSystem.Application.Models.Departments;
using LeaveManagementSystem.Application.Models.Employees;

namespace LeaveManagementSystem.Application.Services.Departments
{
    public interface IDepartmentsService
    {
        Task<DepartmentsReadOnlyListVM> GetAllDepartments();
        Task<List<EmployeeListVM>> GetManagers();
        Task CreateDepartment(DepartmentCreateVM departmentCreateVM);
        Task <Department> GetDepartmentById(int? id);
        Task <ApplicationUser> GetDepartmentManager(int? departmentId);

    }
}