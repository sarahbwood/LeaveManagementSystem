using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LeaveManagementSystem.Application.Models.Departments;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.Departments
{
    public class DepartmentsService(LeaveManagementSystemWebContext _context, IMapper _mapper, IUserService _userService) : IDepartmentsService
    {
        public async Task<DepartmentsReadOnlyListVM> GetAllDepartments()
        {
            var departments = await _context.Departments
                .Include(q => q.DepartmentManager)
                .ToListAsync();

            var model = new DepartmentsReadOnlyListVM
            {
               Departments = departments
                .Select(
                   q => new DepartmentReadOnlyVM
                   {
                       Id = q.Id,
                       DepartmentName = q.DepartmentName,
                       DepartmentManagerId = q.DepartmentManagerId,
                       DepartmentManagerName = $"{q.DepartmentManager.FirstName} {q.DepartmentManager.LastName}"
                   }
                )
                .ToList(),
            };

            return model;

        }

        public async Task<List<EmployeeListVM>> GetManagers()
        {
            var managers = await _userService.GetManagers();
            return _mapper.Map<List<EmployeeListVM>>(managers);
        }

        public async Task CreateDepartment(DepartmentCreateVM departmentCreateVM)
        {
            var department = _mapper.Map<Department>(departmentCreateVM);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }
    }
}
