namespace LeaveManagementSystem.Application.Models.LeaveTypes
{
    public class LeaveTypeCreateVM
    {
        // below attr are not nullable
        [Required]
        [Length(5, 150, ErrorMessage = "Leave Type Name must be between 5 and 150 characters.")]
        public string LeaveTypeName { get; set; } = string.Empty;

        [Required]
        [Range(1, 90, ErrorMessage = "Number of days must be between 1 and 90 days.")]
        public int NumberOfDays { get; set; }
        // Validation attributes can be added here, e.g.:
        // [Required(ErrorMessage = "Leave Type Name is required")]
        // [StringLength(150, ErrorMessage = "Leave Type Name cannot exceed 150 characters")]
        // [Range(1, 365, ErrorMessage = "Number of Days must be between 1 and 365")]
    }
}
