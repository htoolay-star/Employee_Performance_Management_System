using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveEmailFromContactToProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                schema: "hr",
                table: "EmployeeContacts");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                schema: "hr",
                table: "EmployeeProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                schema: "hr",
                table: "EmployeeProfiles");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                schema: "hr",
                table: "EmployeeContacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
