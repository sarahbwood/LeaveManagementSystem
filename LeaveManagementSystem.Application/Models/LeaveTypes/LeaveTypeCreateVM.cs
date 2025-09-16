namespace LeaveManagementSystem.Application.Models.LeaveTypes
{
    public class LeaveTypeCreateVM
    {
        // below attr are not nullable
        [Required]
        [Length(5, 150, ErrorMessage = "Leave Type Name must be between 5 and 150 characters.")]
        [Display(Name = "Leave Type Name")]
        public string LeaveTypeName { get; set; } = string.Empty;
    }
}
