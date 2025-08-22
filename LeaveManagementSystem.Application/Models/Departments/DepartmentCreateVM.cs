using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Application.Models.Departments
{
    public class DepartmentCreateVM
    {
        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Department Manager")]

        public string? DepartmentManagerId { get; set; }
        [Required]
        public SelectList? DepartmentManagers { get; set; }
    }
}
