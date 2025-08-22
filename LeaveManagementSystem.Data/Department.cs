using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Data
{
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; } 
        public string DepartmentManagerId { get; set; } // This is the ID of the department manager
        public ApplicationUser? DepartmentManager { get; set; } // Navigation property for DepartmentManager - like a foreign key

    }
}
