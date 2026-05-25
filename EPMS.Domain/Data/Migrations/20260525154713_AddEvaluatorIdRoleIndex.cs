using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluatorIdRoleIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EvaluationResponses_EvaluatorId",
                schema: "perf",
                table: "EvaluationResponses");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationResponses_EvaluatorId_EvaluatorRole",
                schema: "perf",
                table: "EvaluationResponses",
                columns: new[] { "EvaluatorId", "EvaluatorRole" },
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EvaluationResponses_EvaluatorId_EvaluatorRole",
                schema: "perf",
                table: "EvaluationResponses");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationResponses_EvaluatorId",
                schema: "perf",
                table: "EvaluationResponses",
                column: "EvaluatorId");
        }
    }
}
