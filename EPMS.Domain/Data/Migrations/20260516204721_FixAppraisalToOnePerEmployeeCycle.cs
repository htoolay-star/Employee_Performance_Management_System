using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixAppraisalToOnePerEmployeeCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appraisals_EmployeeProfiles_AppraiserId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropIndex(
                name: "IX_Appraisals_EmployeeId_CycleId_EvaluatorRole",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "EvaluatorRole",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.RenameColumn(
                name: "AppraiserId",
                schema: "perf",
                table: "Appraisals",
                newName: "ManagerReviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_Appraisals_AppraiserId",
                schema: "perf",
                table: "Appraisals",
                newName: "IX_Appraisals_ManagerReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_EmployeeId_CycleId",
                schema: "perf",
                table: "Appraisals",
                columns: new[] { "EmployeeId", "CycleId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Appraisals_EmployeeProfiles_ManagerReviewerId",
                schema: "perf",
                table: "Appraisals",
                column: "ManagerReviewerId",
                principalSchema: "hr",
                principalTable: "EmployeeProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appraisals_EmployeeProfiles_ManagerReviewerId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropIndex(
                name: "IX_Appraisals_EmployeeId_CycleId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.RenameColumn(
                name: "ManagerReviewerId",
                schema: "perf",
                table: "Appraisals",
                newName: "AppraiserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appraisals_ManagerReviewerId",
                schema: "perf",
                table: "Appraisals",
                newName: "IX_Appraisals_AppraiserId");

            migrationBuilder.AddColumn<string>(
                name: "EvaluatorRole",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_EmployeeId_CycleId_EvaluatorRole",
                schema: "perf",
                table: "Appraisals",
                columns: new[] { "EmployeeId", "CycleId", "EvaluatorRole" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Appraisals_EmployeeProfiles_AppraiserId",
                schema: "perf",
                table: "Appraisals",
                column: "AppraiserId",
                principalSchema: "hr",
                principalTable: "EmployeeProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
