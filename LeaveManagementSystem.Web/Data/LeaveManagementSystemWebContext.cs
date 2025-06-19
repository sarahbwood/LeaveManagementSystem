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

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "961eec61-3b7b-4069-880f-761dd206ce89",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole
            {
                Id = "0fbeaf78-3c5d-4926-86d9-63aa09f30fc3",
                Name = "Supervisor",
                NormalizedName = "SUPERVISOR"
            },
            new IdentityRole
            {
                Id = "7ae9914c-8488-4c6f-8fed-465d7e31707a",
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            }

        );

        builder.Entity<IdentityUser>().HasData(
            new IdentityUser
            {
                Id = "71d6b0b7-6d31-437c-957d-736461c0041d",
                UserName = "admin@localhost.com",
                NormalizedUserName = "ADMIN@LOCALHOST.COM",
                Email = "admin@localhost.com",
                NormalizedEmail = "ADMIN@LOCALHOST.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEBW7ac8vdc6z8wTbTWJQ9Wch5Od0B7wNjHptnC7JM/0RVYQk4GLC0zlYVwH9Wix2EQ==",
                EmailConfirmed = true
            }
        );

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "961eec61-3b7b-4069-880f-761dd206ce89",
                UserId = "71d6b0b7-6d31-437c-957d-736461c0041d"
            }
        );

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
