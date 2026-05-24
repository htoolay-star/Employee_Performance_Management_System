using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAppraisalWeightDefaultTo25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AppraisalWeight",
                schema: "perf",
                table: "AppraisalCycles",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 25m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldDefaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AppraisalWeight",
                schema: "perf",
                table: "AppraisalCycles",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldDefaultValue: 25m);
        }
    }
}
