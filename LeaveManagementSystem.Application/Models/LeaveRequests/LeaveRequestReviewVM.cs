using LeaveManagementSystem.Application.Models.LeaveAllocations;

namespace LeaveManagementSystem.Application.Models.LeaveRequests
{
    public class LeaveRequestReviewVM : LeaveRequestReadOnlyVM
    {
        public EmployeeListVM Employee { get; set; } = new EmployeeListVM();
        [Display(Name = "Additional Comments")]
        public string RequestComments { get; set; } = string.Empty;
    }
}