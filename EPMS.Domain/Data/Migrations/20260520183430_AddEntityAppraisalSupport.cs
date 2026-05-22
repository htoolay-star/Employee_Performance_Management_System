using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityAppraisalSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appraisals_EmployeeId_CycleId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                schema: "perf",
                table: "Appraisals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "EntityId",
                schema: "perf",
                table: "Appraisals",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                schema: "perf",
                table: "Appraisals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_EmployeeId_CycleId",
                schema: "perf",
                table: "Appraisals",
                columns: new[] { "EmployeeId", "CycleId" },
                unique: true,
                filter: "[IsDeleted] = 0 AND [EmployeeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_EntityType_EntityId_CycleId",
                schema: "perf",
                table: "Appraisals",
                columns: new[] { "EntityType", "EntityId", "CycleId" },
                unique: true,
                filter: "[IsDeleted] = 0 AND [EntityType] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appraisals_EmployeeId_CycleId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropIndex(
                name: "IX_Appraisals_EntityType_EntityId_CycleId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "EntityType",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                schema: "perf",
                table: "Appraisals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_EmployeeId_CycleId",
                schema: "perf",
                table: "Appraisals",
                columns: new[] { "EmployeeId", "CycleId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
