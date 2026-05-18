using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveYesNoCommentToTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasComment",
                schema: "perf",
                table: "FormQuestions");

            migrationBuilder.DropColumn(
                name: "HasYesNo",
                schema: "perf",
                table: "FormQuestions");

            migrationBuilder.AddColumn<bool>(
                name: "HasComment",
                schema: "perf",
                table: "FormTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasYesNo",
                schema: "perf",
                table: "FormTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasComment",
                schema: "perf",
                table: "FormTemplates");

            migrationBuilder.DropColumn(
                name: "HasYesNo",
                schema: "perf",
                table: "FormTemplates");

            migrationBuilder.AddColumn<bool>(
                name: "HasComment",
                schema: "perf",
                table: "FormQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasYesNo",
                schema: "perf",
                table: "FormQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
