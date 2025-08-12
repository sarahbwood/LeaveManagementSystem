using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Data
{
    public class LeaveType
    {
        public int LeaveTypeId { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string LeaveTypeName { get; set; }
        public int NumberOfDays { get; set; }

        public List<LeaveAllocation>? LeaveAllocations { get; set; } // Navigation property for related LeaveAllocations
    }
}