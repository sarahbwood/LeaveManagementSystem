using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Models.Periods;

namespace LeaveManagementSystem.Web.Models.LeaveAllocations
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }
        [Display(Name = "Number of Days")]
        public int NumberOfDays { get; set; }
        [Display(Name = "Allocation Period")]
        public PeriodReadOnlyVM Period { get; set; } = new PeriodReadOnlyVM();
        public LeaveTypeReadOnlyVM LeaveType { get; set; } = new LeaveTypeReadOnlyVM();



    }
}
