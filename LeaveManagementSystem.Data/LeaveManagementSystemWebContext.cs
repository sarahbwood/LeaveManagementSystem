using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Data;

public class LeaveManagementSystemWebContext : IdentityDbContext<ApplicationUser>
{
    public LeaveManagementSystemWebContext(DbContextOptions<LeaveManagementSystemWebContext> options)
        : base(options)
    {
    }
    // Add a DbSet for each entity type that you want to include in the model.
    // Db<type> tableName
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
    public DbSet<LeaveRequestStatus> LeaveRequestsStatuses { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // this line is important to ensure the Identity tables are created correctly
        builder.Entity<Department>()
            .HasOne(d => d.DepartmentManager)
            .WithOne(u => u.Department) 
            .HasForeignKey<Department>(d => d.DepartmentManagerId);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Automatically apply all configurations in the current assembly - implementing IEntityTypeConfiguration<T> interface
    }
}
