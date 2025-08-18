using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedManagerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71d6b0b7-6d31-437c-957d-736461c0041d",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9e6a5686-0ed2-41da-80eb-4e37af582980", "fcb87cfd-2b29-43a4-a448-b4014f2c2afd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71d6b0b7-6d31-437c-957d-736461c0041d",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2d38903c-bc91-4275-a169-7b461a982ad0", "8c3b7693-5f5f-45be-b0c4-3b478fdceb44" });
        }
    }
}
