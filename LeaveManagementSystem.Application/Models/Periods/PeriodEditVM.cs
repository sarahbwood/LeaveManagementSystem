namespace LeaveManagementSystem.Application.Models.Periods
{
    public class PeriodEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }
    }
}
