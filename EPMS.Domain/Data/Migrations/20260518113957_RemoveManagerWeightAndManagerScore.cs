using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveManagerWeightAndManagerScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerScore",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "ManagerWeight",
                schema: "perf",
                table: "AppraisalCycles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ManagerScore",
                schema: "perf",
                table: "Appraisals",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ManagerWeight",
                schema: "perf",
                table: "AppraisalCycles",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 25m);
        }
    }
}
