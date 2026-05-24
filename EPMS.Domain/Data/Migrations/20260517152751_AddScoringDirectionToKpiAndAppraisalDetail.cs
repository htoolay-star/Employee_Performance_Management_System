using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScoringDirectionToKpiAndAppraisalDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScoringDirection",
                schema: "perf",
                table: "KPIMaster",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "HigherIsBetter");

            migrationBuilder.AddColumn<string>(
                name: "ScoringDirection",
                schema: "perf",
                table: "AppraisalDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "HigherIsBetter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoringDirection",
                schema: "perf",
                table: "KPIMaster");

            migrationBuilder.DropColumn(
                name: "ScoringDirection",
                schema: "perf",
                table: "AppraisalDetails");
        }
    }
}
