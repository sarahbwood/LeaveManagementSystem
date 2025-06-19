using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class IdentityRelatedDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0fbeaf78-3c5d-4926-86d9-63aa09f30fc3", null, "Supervisor", "SUPERVISOR" },
                    { "7ae9914c-8488-4c6f-8fed-465d7e31707a", null, "Employee", "EMPLOYEE" },
                    { "961eec61-3b7b-4069-880f-761dd206ce89", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "71d6b0b7-6d31-437c-957d-736461c0041d", 0, "2220cb01-d5b9-461c-b721-7ce1f7802640", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEBW7ac8vdc6z8wTbTWJQ9Wch5Od0B7wNjHptnC7JM/0RVYQk4GLC0zlYVwH9Wix2EQ==", null, false, "14910b29-4bf0-489d-a3b4-65fedb4dade2", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "961eec61-3b7b-4069-880f-761dd206ce89", "71d6b0b7-6d31-437c-957d-736461c0041d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0fbeaf78-3c5d-4926-86d9-63aa09f30fc3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ae9914c-8488-4c6f-8fed-465d7e31707a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "961eec61-3b7b-4069-880f-761dd206ce89", "71d6b0b7-6d31-437c-957d-736461c0041d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "961eec61-3b7b-4069-880f-761dd206ce89");
        }
    }
}
