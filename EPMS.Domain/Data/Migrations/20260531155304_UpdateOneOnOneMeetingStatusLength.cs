using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOneOnOneMeetingStatusLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "perf",
                table: "OneOnOneMeetings",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "PENDING_EMPLOYEE_CONFIRMATION",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Scheduled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "perf",
                table: "OneOnOneMeetings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Scheduled",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "PENDING_EMPLOYEE_CONFIRMATION");
        }
    }
}
