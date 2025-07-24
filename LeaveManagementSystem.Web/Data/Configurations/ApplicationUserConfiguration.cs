using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(
                new ApplicationUser
                {
                    Id = "71d6b0b7-6d31-437c-957d-736461c0041d",
                    UserName = "admin@localhost.com",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEBW7ac8vdc6z8wTbTWJQ9Wch5Od0B7wNjHptnC7JM/0RVYQk4GLC0zlYVwH9Wix2EQ==",
                    EmailConfirmed = true,
                    FirstName = "Default",
                    LastName = "Administrator",
                    DateOfBirth = new DateOnly(1970, 1, 1)
                }
            );
        }
    }
}
