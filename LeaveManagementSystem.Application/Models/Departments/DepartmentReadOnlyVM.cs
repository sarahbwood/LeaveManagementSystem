using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.Models.Departments
{
    public class DepartmentReadOnlyVM
    {
        public int Id { get; set; }
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public string DepartmentManagerId { get; set; } // This is the ID of the department manager
        [Display(Name = "Department Manager")]
        public string DepartmentManagerName { get; set; } = string.Empty;
    }
}
