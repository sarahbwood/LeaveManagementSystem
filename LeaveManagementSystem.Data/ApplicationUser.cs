namespace LeaveManagementSystem.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int? DepartmentId { get; set; } // Foreign key
        public Department? Department { get; set; } // Navigation property
        public bool IsActive { get; set; } = true;
    }
}
