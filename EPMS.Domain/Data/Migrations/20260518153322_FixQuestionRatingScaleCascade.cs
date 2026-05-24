using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixQuestionRatingScaleCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionRatingScaleLevels_QuestionRatingScales_QuestionRatingScaleId",
                schema: "perf",
                table: "QuestionRatingScaleLevels");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionRatingScaleLevels_QuestionRatingScales_QuestionRatingScaleId",
                schema: "perf",
                table: "QuestionRatingScaleLevels",
                column: "QuestionRatingScaleId",
                principalSchema: "perf",
                principalTable: "QuestionRatingScales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionRatingScaleLevels_QuestionRatingScales_QuestionRatingScaleId",
                schema: "perf",
                table: "QuestionRatingScaleLevels");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionRatingScaleLevels_QuestionRatingScales_QuestionRatingScaleId",
                schema: "perf",
                table: "QuestionRatingScaleLevels",
                column: "QuestionRatingScaleId",
                principalSchema: "perf",
                principalTable: "QuestionRatingScales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
