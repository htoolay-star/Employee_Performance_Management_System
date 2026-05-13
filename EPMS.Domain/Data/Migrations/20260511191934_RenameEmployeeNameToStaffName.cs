using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameEmployeeNameToStaffName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "hr",
                table: "EmployeeProfiles");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "hr",
                table: "EmployeeProfiles",
                newName: "StaffName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StaffName",
                schema: "hr",
                table: "EmployeeProfiles",
                newName: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "hr",
                table: "EmployeeProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
