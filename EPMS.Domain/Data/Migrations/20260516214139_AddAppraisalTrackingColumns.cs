using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppraisalTrackingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormulaWeights",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "KpiScore",
                schema: "perf",
                table: "Appraisals",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ManagerScore",
                schema: "perf",
                table: "Appraisals",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PeerScore",
                schema: "perf",
                table: "Appraisals",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SelfScore",
                schema: "perf",
                table: "Appraisals",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormulaWeights",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "KpiScore",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "ManagerScore",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "PeerScore",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "SelfScore",
                schema: "perf",
                table: "Appraisals");
        }
    }
}
