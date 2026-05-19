using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class MergeManagerInto360Review : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerLockIsDeadline",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "ManagerLocked",
                schema: "perf",
                table: "Appraisals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ManagerLockIsDeadline",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ManagerLocked",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
