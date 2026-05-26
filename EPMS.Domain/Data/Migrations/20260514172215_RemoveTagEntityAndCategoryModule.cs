using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTagEntityAndCategoryModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormQuestionTags",
                schema: "perf");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "shared");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Module_Code",
                schema: "shared",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Module",
                schema: "shared",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                schema: "shared",
                table: "Categories",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Code",
                schema: "shared",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Module",
                schema: "shared",
                table: "Categories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "shared",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormQuestionTags",
                schema: "perf",
                columns: table => new
                {
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormQuestionTags", x => new { x.QuestionId, x.TagId });
                    table.ForeignKey(
                        name: "FK_FormQuestionTags_FormQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "perf",
                        principalTable: "FormQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormQuestionTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "shared",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Module_Code",
                schema: "shared",
                table: "Categories",
                columns: new[] { "Module", "Code" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_FormQuestionTags_TagId",
                schema: "perf",
                table: "FormQuestionTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Code",
                schema: "shared",
                table: "Tags",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_IsActive",
                schema: "shared",
                table: "Tags",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "shared",
                table: "Tags",
                column: "Name",
                unique: true);
        }
    }
}
