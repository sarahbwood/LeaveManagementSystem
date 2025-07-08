﻿namespace LeaveManagementSystem.Web.Models.LeaveAllocations
{
    public class EmployeeAllocationVM : EmployeeListVM
    {
       
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        public bool IsCompletedAllocation { get; set; }
        
        // view models should only refer to other view models, not data models
        public List<LeaveAllocationVM> LeaveAllocations { get; set; } = new List<LeaveAllocationVM>();
    }
}
