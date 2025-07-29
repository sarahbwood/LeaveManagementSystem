using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestCreateVM : IValidatableObject
    {
        [Display(Name="Start Date")]
        [Required]
        public DateOnly StartDate { get; set; }
        [Display(Name="End Date")]
        [Required]
        public DateOnly EndDate { get; set; }
        [Display(Name = "Leave Type")]
        [Required]
        public int LeaveTypeId { get; set; }
        [Display(Name="Additional Comments")]
        public string? RequestComments { get; set; }
        public SelectList? LeaveTypes { get; set; } // not in data model, therefore not in mapping profile

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) // for validations that don't require a db call & more complex than an annotation
        {
            if (StartDate > EndDate)
            {
                yield return new ValidationResult("The start date must be before the end date.", [nameof(StartDate), nameof(EndDate)]);
            }
        }
    }
}