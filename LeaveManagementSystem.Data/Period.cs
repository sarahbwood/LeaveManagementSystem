namespace LeaveManagementSystem.Data
{
    public class Period : BaseEntity // Inheriting from BaseEntity to get Id property
    {
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

    }
}
