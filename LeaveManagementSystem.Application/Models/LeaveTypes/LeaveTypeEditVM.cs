namespace LeaveManagementSystem.Application.Models.LeaveTypes
{
    public class LeaveTypeEditVM : BaseLeaveTypeVM
    {
        [Required]
        [Length(5, 150, ErrorMessage = "Leave Type Name must be between 5 and 150 characters.")]
        [Display(Name = "Leave Type")]
        public string LeaveTypeName { get; set; } = string.Empty;
    }
}
