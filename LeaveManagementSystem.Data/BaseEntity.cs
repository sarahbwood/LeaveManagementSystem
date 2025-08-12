namespace LeaveManagementSystem.Data
{
    public abstract class BaseEntity // abstract classes cannot be instantiated, 
                                     // but can be used as a base class for other classes
    {
        public int Id { get; set; }
    }
}
