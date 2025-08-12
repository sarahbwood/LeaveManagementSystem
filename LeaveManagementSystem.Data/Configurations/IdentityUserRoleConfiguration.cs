using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Data.Configurations
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>> // key type is string for IdentityUserRole - this is the default for ASP.NET Identity
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                 new IdentityUserRole<string>
                 {
                     RoleId = "961eec61-3b7b-4069-880f-761dd206ce89",
                     UserId = "71d6b0b7-6d31-437c-957d-736461c0041d"
                 }
            );
        }
    }
}
