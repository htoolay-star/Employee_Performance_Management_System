using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppraisalWeightAndReviewerAndScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppraisalLockIsDeadline",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AppraisalLocked",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "AppraisalScore",
                schema: "perf",
                table: "Appraisals",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AppraisalReviewerId",
                schema: "perf",
                table: "AppraisalCycles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AppraisalWeight",
                schema: "perf",
                table: "AppraisalCycles",
                type: "decimal(18,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalCycles_AppraisalReviewerId",
                schema: "perf",
                table: "AppraisalCycles",
                column: "AppraisalReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalCycles_EmployeeProfiles_AppraisalReviewerId",
                schema: "perf",
                table: "AppraisalCycles",
                column: "AppraisalReviewerId",
                principalSchema: "hr",
                principalTable: "EmployeeProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalCycles_EmployeeProfiles_AppraisalReviewerId",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropIndex(
                name: "IX_AppraisalCycles_AppraisalReviewerId",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "AppraisalLockIsDeadline",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "AppraisalLocked",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "AppraisalScore",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "AppraisalReviewerId",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "AppraisalWeight",
                schema: "perf",
                table: "AppraisalCycles");
        }
    }
}
