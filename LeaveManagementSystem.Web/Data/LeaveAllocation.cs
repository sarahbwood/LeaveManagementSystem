namespace LeaveManagementSystem.Web.Data
{
    public class LeaveAllocation : BaseEntity 
    {
        public LeaveType? LeaveType { get; set; } // Navigation property
        public int LeaveTypeId { get; set; } // Foreign key
        public ApplicationUser? Employee { get; set; } // Navigation property
        public string EmployeeId { get; set; } // Foreign key 
                                               // User Id is a guid in ASP.NET Identity, so we use string here

        public Period? Period { get; set; } // Navigation property
        public int PeriodId { get; set; } // Foreign key
        public int NumberOfDays { get; set; } // Number of days allocated for the leave type

    }
}
