using System.ComponentModel;
using LeaveManagementSystem.Web.Services.LeaveRequests;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestReadOnlyVM
    {
        public int Id { get; set; }
        [Display(Name = "Start Date")]
        public DateOnly StartDate{ get; set; }
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }
        [Display(Name = "Total Days")]
        public int NumberOfDays { get; set; }
        [Display(Name = "Leave Type")]
        public string LeaveType { get; set; } = string.Empty;
        [Display(Name = "Status")]
        public LeaveRequestStatusEnum LeaveRequestStatus { get; set; } 
    }
}