using LeaveManagementSystem.Application.Models.Employees;
using LeaveManagementSystem.Application.Services.Departments;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.EntityFrameworkCore;


namespace LeaveManagementSystem.Application.Services.Employees
{
    public class EmployeesService(LeaveManagementSystemWebContext _context, IUserService _userService, IMapper _mapper, IDepartmentsService _departmentsService) : IEmployeesService
    {
        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId) ? await _userService.GetLoggedInUser()
                : await _userService.GetUserById(userId);
            var department = await _departmentsService.GetDepartmentById(user.DepartmentId);
          
            var employeeVM = new EmployeeAllocationVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                DepartmentName = department.DepartmentName,
            };

            return employeeVM;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userService.GetEmployees();
            var employees = _mapper.Map<List<EmployeeListVM>>(users);


            return employees;
        }
    }
}
