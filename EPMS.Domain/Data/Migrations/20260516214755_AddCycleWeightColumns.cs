using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCycleWeightColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "KpiWeight",
                schema: "perf",
                table: "AppraisalCycles",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 50m);

            migrationBuilder.AddColumn<decimal>(
                name: "ManagerWeight",
                schema: "perf",
                table: "AppraisalCycles",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 25m);

            migrationBuilder.AddColumn<decimal>(
                name: "PeerWeight",
                schema: "perf",
                table: "AppraisalCycles",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 10m);

            migrationBuilder.AddColumn<decimal>(
                name: "SelfWeight",
                schema: "perf",
                table: "AppraisalCycles",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 15m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KpiWeight",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "ManagerWeight",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "PeerWeight",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "SelfWeight",
                schema: "perf",
                table: "AppraisalCycles");
        }
    }
}
