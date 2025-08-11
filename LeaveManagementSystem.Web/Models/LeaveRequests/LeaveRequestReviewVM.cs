namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestReviewVM : LeaveRequestReadOnlyVM
    {
        public EmployeeListVM Employee { get; set; } = new EmployeeListVM();
        [Display(Name = "Additional Comments")]
        public string RequestComments { get; set; } = string.Empty;
    }
}