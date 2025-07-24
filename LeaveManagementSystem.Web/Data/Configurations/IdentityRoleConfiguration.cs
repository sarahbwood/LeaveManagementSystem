using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
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
        }
    }
}
