using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class LeaveTypeEditVM : BaseLeaveTypeVM
    {
        [Required]
        [Length(5, 150, ErrorMessage = "Leave Type Name must be between 5 and 150 characters.")]
        [Display(Name="Leave Type")]
        public string LeaveTypeName { get; set; } = string.Empty;

        [Required]
        [Range(1, 90, ErrorMessage = "Number of days must be between 1 and 90 days.")]
        [Display(Name ="Number of Days")]
        public int NumberOfDays { get; set; }
    }
}
