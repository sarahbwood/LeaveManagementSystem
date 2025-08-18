using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewIndentityARoleManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bf9b59c8-9131-401b-a9c8-4d5329189307", null, "Manager", "MANAGER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71d6b0b7-6d31-437c-957d-736461c0041d",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2984d871-869b-4b45-b969-aa98aadc8fe9", "c3e9b9b9-a6ab-49ba-8cd2-7a6dd094e6aa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf9b59c8-9131-401b-a9c8-4d5329189307");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71d6b0b7-6d31-437c-957d-736461c0041d",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9e6a5686-0ed2-41da-80eb-4e37af582980", "fcb87cfd-2b29-43a4-a448-b4014f2c2afd" });
        }
    }
}
