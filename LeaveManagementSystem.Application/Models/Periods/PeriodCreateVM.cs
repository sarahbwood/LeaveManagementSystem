namespace LeaveManagementSystem.Application.Models.Periods
{
    public class PeriodCreateVM
    {
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }
    }
}
