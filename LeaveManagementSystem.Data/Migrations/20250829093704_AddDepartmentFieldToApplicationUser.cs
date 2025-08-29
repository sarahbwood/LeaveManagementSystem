using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentFieldToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_DepartmentManagerId",
                table: "Departments");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71d6b0b7-6d31-437c-957d-736461c0041d",
                columns: new[] { "ConcurrencyStamp", "DepartmentId", "IsActive", "SecurityStamp" },
                values: new object[] { "fead2f6c-915a-4ba5-bc59-066c40e0b53a", null, true, "220fa490-3e39-45fd-b6b6-9d8aaacab05e" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentManagerId",
                table: "Departments",
                column: "DepartmentManagerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_DepartmentManagerId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71d6b0b7-6d31-437c-957d-736461c0041d",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b8fa583a-932b-410a-8ef2-a47f7a1210b2", "4b26aad8-8bff-4e17-b2ce-c7f538d68a6a" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentManagerId",
                table: "Departments",
                column: "DepartmentManagerId");
        }
    }
}
