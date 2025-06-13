using LeaveManagementSystem.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data;

public class LeaveManagementSystemWebContext : IdentityDbContext<LeaveManagementSystemWebUser>
{
    public LeaveManagementSystemWebContext(DbContextOptions<LeaveManagementSystemWebContext> options)
        : base(options)
    {
    }
    // Add a DbSet for each entity type that you want to include in the model.
    // Db<type> tableName
    public DbSet<LeaveType> LeaveTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
