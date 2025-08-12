namespace LeaveManagementSystem.Data
{
    public class LeaveRequest : BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public LeaveType? LeaveType { get; set; } // Navigation property for LeaveType - like a foreign key
        public int LeaveTypeId { get; set; }
        public LeaveRequestStatus? LeaveRequestsStatus { get; set; } // Navigation property for LeaveRequestsStatus - like a foreign key
        public int LeaveRequestStatusId { get; set; }
        public ApplicationUser? Employee { get; set; } // Navigation property for Employee - like a foreign key
        public string EmployeeId { get; set; }
        public ApplicationUser? Reviewer { get; set; } // Navigation property for Reviewer - like a foreign key
        public string? ReviewerId { get; set; } // must be nullable to allow for initial requests without a reviewer - referential integrity is maintained by the database
        public string? RequestComments { get; set; }
    }
}