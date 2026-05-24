using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAppraisalReviewerIdFromCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "AppraisalReviewerId",
                schema: "perf",
                table: "AppraisalCycles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AppraisalReviewerId",
                schema: "perf",
                table: "AppraisalCycles",
                type: "bigint",
                nullable: true);

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
    }
}
