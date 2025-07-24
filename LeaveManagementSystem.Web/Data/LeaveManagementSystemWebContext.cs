using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data;

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
    public DbSet<Period> Periods { get; set; }
    public DbSet<LeaveRequestStatus> LeaveRequestsStatuses { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

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

        // The following code works, but there is a more succint way to configure the Identity tables
        //builder.ApplyConfiguration(new Configurations.IdentityRoleConfiguration());
        //builder.ApplyConfiguration(new Configurations.ApplicationUserConfiguration());
        //builder.ApplyConfiguration(new Configurations.IdentityUserRoleConfiguration());
        //builder.ApplyConfiguration(new Configurations.LeaveRequestStatusConfiguration());
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Automatically apply all configurations in the current assembly - implementing IEntityTypeConfiguration<T> interface

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
