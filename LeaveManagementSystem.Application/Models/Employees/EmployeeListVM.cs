namespace LeaveManagementSystem.Application.Models.Employees
{
    public class EmployeeListVM
    {
        public string Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        [Display(Name = "Department")]
        public string DepartmentName { get; set; } = string.Empty;
    }
}
