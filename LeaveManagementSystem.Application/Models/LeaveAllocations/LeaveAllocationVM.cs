using LeaveManagementSystem.Application.Models.LeaveTypes;
using LeaveManagementSystem.Application.Models.Periods;

namespace LeaveManagementSystem.Application.Models.LeaveAllocations
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
