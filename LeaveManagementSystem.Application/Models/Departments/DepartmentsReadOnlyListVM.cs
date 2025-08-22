using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.Models.Departments
{
    public class DepartmentsReadOnlyListVM
    {
        public List<DepartmentReadOnlyVM> Departments { get; set; } = new List<DepartmentReadOnlyVM>();
    }
}
