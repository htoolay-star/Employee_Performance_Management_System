using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQuestionIdFromAppraisalDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalDetails_FormQuestions_QuestionId",
                schema: "perf",
                table: "AppraisalDetails");

            migrationBuilder.DropIndex(
                name: "IX_AppraisalDetails_QuestionId",
                schema: "perf",
                table: "AppraisalDetails");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                schema: "perf",
                table: "AppraisalDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "QuestionId",
                schema: "perf",
                table: "AppraisalDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalDetails_QuestionId",
                schema: "perf",
                table: "AppraisalDetails",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalDetails_FormQuestions_QuestionId",
                schema: "perf",
                table: "AppraisalDetails",
                column: "QuestionId",
                principalSchema: "perf",
                principalTable: "FormQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
