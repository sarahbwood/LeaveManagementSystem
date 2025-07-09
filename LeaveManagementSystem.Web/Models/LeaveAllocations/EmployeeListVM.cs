namespace LeaveManagementSystem.Web.Models.LeaveAllocations
{
    public class EmployeeListVM
    {
        public string Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;    
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
