using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixAppraisalColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add EvaluatorRole to Appraisals table
            migrationBuilder.AddColumn<string>(
                name: "EvaluatorRole",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            // 2. Add FinalRatingId to Appraisals table
            migrationBuilder.AddColumn<int>(
                name: "FinalRatingId",
                schema: "perf",
                table: "Appraisals",
                type: "int",
                nullable: true);

            // 3. Add QuestionId to AppraisalDetails table
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                schema: "perf",
                table: "AppraisalDetails",
                type: "int",
                nullable: true);

            // 4. Create Foreign Key for FinalRatingId
            migrationBuilder.AddForeignKey(
                name: "FK_Appraisals_RatingScales_FinalRatingId",
                schema: "perf",
                table: "Appraisals",
                column: "FinalRatingId",
                principalSchema: "hr",
                principalTable: "RatingScales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // 5. Create Foreign Key for QuestionId
            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalDetails_FormQuestions_QuestionId",
                schema: "perf",
                table: "AppraisalDetails",
                column: "QuestionId",
                principalSchema: "perf",
                principalTable: "FormQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // 6. Create Indexes
            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_FinalRatingId",
                schema: "perf",
                table: "Appraisals",
                column: "FinalRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalDetails_QuestionId",
                schema: "perf",
                table: "AppraisalDetails",
                column: "QuestionId");

            /* 
            // Update unique index on Appraisals to include EvaluatorRole
            migrationBuilder.DropIndex(
                name: "IX_Appraisals_EmployeeId_CycleId",
                schema: "perf",
                table: "Appraisals");
            */

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_EmployeeId_CycleId_EvaluatorRole",
                schema: "perf",
                table: "Appraisals",
                columns: new[] { "EmployeeId", "CycleId", "EvaluatorRole" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appraisals_RatingScales_FinalRatingId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalDetails_FormQuestions_QuestionId",
                schema: "perf",
                table: "AppraisalDetails");

            migrationBuilder.DropIndex(
                name: "IX_Appraisals_FinalRatingId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropIndex(
                name: "IX_AppraisalDetails_QuestionId",
                schema: "perf",
                table: "AppraisalDetails");

            migrationBuilder.DropIndex(
                name: "IX_Appraisals_EmployeeId_CycleId_EvaluatorRole",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_EmployeeId_CycleId",
                schema: "perf",
                table: "Appraisals",
                columns: new[] { "EmployeeId", "CycleId" },
                unique: true);

            migrationBuilder.DropColumn(
                name: "EvaluatorRole",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "FinalRatingId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                schema: "perf",
                table: "AppraisalDetails");
        }
    }
}
