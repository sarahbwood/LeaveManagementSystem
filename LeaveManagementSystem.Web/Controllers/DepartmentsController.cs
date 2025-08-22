using LeaveManagementSystem.Application.Services.Departments;
using LeaveManagementSystem.Application.Models.Departments;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    public class DepartmentsController(IDepartmentsService _departmentsService) : Controller
    {
        // list all departments
        [Authorize(Roles = Roles.Administrator)] // TODO : Update to use policy-based authorization
        public async Task<IActionResult> Index()
        {
            var model = await _departmentsService.GetAllDepartments();
            return View(model);
        }

       // create a new department
       [Authorize(Roles = Roles.Administrator)] // TODO : Update to use policy-based authorization

        public async Task<IActionResult> Create()
        {
            var managers = await _departmentsService.GetManagers();
            var managersList = new SelectList(managers, "Id", "FullName");

            var model = new DepartmentCreateVM
            {
                DepartmentManagers = managersList,
            };

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(DepartmentCreateVM departmentCreateVM)
        {
            await _departmentsService.CreateDepartment(departmentCreateVM);
            return View(nameof(Index));
        }

    }
}
