using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFormStatusFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommitteeStatus",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "DRAFT");

            migrationBuilder.AddColumn<string>(
                name: "KpiStatus",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "DRAFT");

            migrationBuilder.AddColumn<string>(
                name: "ManagerStatus",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "DRAFT");

            migrationBuilder.AddColumn<string>(
                name: "PeerStatus",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "DRAFT");

            migrationBuilder.AddColumn<string>(
                name: "SelfStatus",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "DRAFT");

            migrationBuilder.AddColumn<string>(
                name: "SubordinateStatus",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "DRAFT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitteeStatus",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "KpiStatus",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "ManagerStatus",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "PeerStatus",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "SelfStatus",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "SubordinateStatus",
                schema: "perf",
                table: "Appraisals");
        }
    }
}
