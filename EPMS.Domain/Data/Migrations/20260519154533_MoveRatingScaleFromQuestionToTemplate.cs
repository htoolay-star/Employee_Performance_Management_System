using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveRatingScaleFromQuestionToTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop FK and index on FormQuestions (column stays for data migration)
            migrationBuilder.DropForeignKey(
                name: "FK_FormQuestions_QuestionRatingScales_QuestionRatingScaleId",
                schema: "perf",
                table: "FormQuestions");

            migrationBuilder.DropIndex(
                name: "IX_FormQuestions_QuestionRatingScaleId",
                schema: "perf",
                table: "FormQuestions");

            // 2. Add column to FormTemplates as nullable
            migrationBuilder.AddColumn<long>(
                name: "QuestionRatingScaleId",
                schema: "perf",
                table: "FormTemplates",
                type: "bigint",
                nullable: true);

            // 3. Copy first non-deleted question's scale to each template
            migrationBuilder.Sql(@"
                UPDATE T
                SET T.QuestionRatingScaleId = SQ.QuestionRatingScaleId
                FROM perf.FormTemplates T
                CROSS APPLY (
                    SELECT TOP 1 Q.QuestionRatingScaleId
                    FROM perf.FormQuestions Q
                    WHERE Q.TemplateId = T.Id AND Q.IsDeleted = 0
                    ORDER BY Q.Sequence
                ) SQ");

            // 4. For templates with no questions, use the first available scale
            migrationBuilder.Sql(@"
                UPDATE T
                SET T.QuestionRatingScaleId = (SELECT TOP 1 Id FROM perf.QuestionRatingScales WHERE IsDeleted = 0)
                FROM perf.FormTemplates T
                WHERE T.QuestionRatingScaleId IS NULL");

            // 5. Drop column from FormQuestions (data already migrated)
            migrationBuilder.DropColumn(
                name: "QuestionRatingScaleId",
                schema: "perf",
                table: "FormQuestions");

            // 6. Make column non-nullable (all rows already populated from steps 3-4)
            migrationBuilder.Sql(@"
                ALTER TABLE [perf].[FormTemplates] ALTER COLUMN [QuestionRatingScaleId] [bigint] NOT NULL");

            // 7. Create index and FK
            migrationBuilder.CreateIndex(
                name: "IX_FormTemplates_QuestionRatingScaleId",
                schema: "perf",
                table: "FormTemplates",
                column: "QuestionRatingScaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormTemplates_QuestionRatingScales_QuestionRatingScaleId",
                schema: "perf",
                table: "FormTemplates",
                column: "QuestionRatingScaleId",
                principalSchema: "perf",
                principalTable: "QuestionRatingScales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Drop FK and index on FormTemplates
            migrationBuilder.DropForeignKey(
                name: "FK_FormTemplates_QuestionRatingScales_QuestionRatingScaleId",
                schema: "perf",
                table: "FormTemplates");

            migrationBuilder.DropIndex(
                name: "IX_FormTemplates_QuestionRatingScaleId",
                schema: "perf",
                table: "FormTemplates");

            // 2. Drop column from FormTemplates
            migrationBuilder.DropColumn(
                name: "QuestionRatingScaleId",
                schema: "perf",
                table: "FormTemplates");

            // 3. Restore column on FormQuestions
            migrationBuilder.AddColumn<long>(
                name: "QuestionRatingScaleId",
                schema: "perf",
                table: "FormQuestions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            // 4. Restore index and FK
            migrationBuilder.CreateIndex(
                name: "IX_FormQuestions_QuestionRatingScaleId",
                schema: "perf",
                table: "FormQuestions",
                column: "QuestionRatingScaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormQuestions_QuestionRatingScales_QuestionRatingScaleId",
                schema: "perf",
                table: "FormQuestions",
                column: "QuestionRatingScaleId",
                principalSchema: "perf",
                principalTable: "QuestionRatingScales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
