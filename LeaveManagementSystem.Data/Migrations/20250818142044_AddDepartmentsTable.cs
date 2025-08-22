using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_AspNetUsers_DepartmentManagerId",
                        column: x => x.DepartmentManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71d6b0b7-6d31-437c-957d-736461c0041d",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2984d871-869b-4b45-b969-aa98aadc8fe9", "c3e9b9b9-a6ab-49ba-8cd2-7a6dd094e6aa" });
        }
    }
}
