namespace LeaveManagementSystem.Application.Models.LeaveTypes
{
    public class LeaveTypeReadOnlyVM : BaseLeaveTypeVM
    {
        [Display(Name = "Leave Type")]
        public string LeaveTypeName { get; set; } = string.Empty;

        [Display(Name = "Number of Days")]
        public int NumberOfDays { get; set; }
    }
}
