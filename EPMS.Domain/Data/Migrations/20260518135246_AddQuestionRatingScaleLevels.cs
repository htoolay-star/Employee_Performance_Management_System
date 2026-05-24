using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    public partial class AddQuestionRatingScaleLevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionRatingScaleLevels",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionRatingScaleId = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    MinScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MaxScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionRatingScaleLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionRatingScaleLevels_QuestionRatingScales_QuestionRatingScaleId",
                        column: x => x.QuestionRatingScaleId,
                        principalSchema: "perf",
                        principalTable: "QuestionRatingScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRatingScaleLevels_PublicId",
                schema: "perf",
                table: "QuestionRatingScaleLevels",
                column: "PublicId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRatingScaleLevels_QuestionRatingScaleId_Rating",
                schema: "perf",
                table: "QuestionRatingScaleLevels",
                columns: new[] { "QuestionRatingScaleId", "Rating" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'perf' AND TABLE_NAME = 'QuestionRatingScales' AND COLUMN_NAME = 'MinScore')
                BEGIN
                    INSERT INTO [perf].[QuestionRatingScaleLevels]
                    (QuestionRatingScaleId, Rating, MinScore, MaxScore, IsDeleted, PublicId, CreatedAt, UpdatedAt)
                    SELECT
                        Id,
                        1,
                        MinScore,
                        MaxScore,
                        0,
                        NEWID(),
                        GETUTCDATE(),
                        GETUTCDATE()
                    FROM [perf].[QuestionRatingScales]
                    WHERE IsDeleted = 0
                END
            ");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                schema: "perf",
                table: "QuestionRatingScales");

            migrationBuilder.DropColumn(
                name: "MinScore",
                schema: "perf",
                table: "QuestionRatingScales");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinScore",
                schema: "perf",
                table: "QuestionRatingScales",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxScore",
                schema: "perf",
                table: "QuestionRatingScales",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(@"
                UPDATE [perf].[QuestionRatingScales]
                SET MinScore = (SELECT TOP 1 MinScore FROM [perf].[QuestionRatingScaleLevels] WHERE QuestionRatingScaleId = QuestionRatingScales.Id ORDER BY Rating ASC),
                    MaxScore = (SELECT TOP 1 MaxScore FROM [perf].[QuestionRatingScaleLevels] WHERE QuestionRatingScaleId = QuestionRatingScales.Id ORDER BY Rating ASC)
                WHERE EXISTS (SELECT 1 FROM [perf].[QuestionRatingScaleLevels] WHERE QuestionRatingScaleId = QuestionRatingScales.Id)
            ");

            migrationBuilder.DropTable(
                name: "QuestionRatingScaleLevels",
                schema: "perf");
        }
    }
}
