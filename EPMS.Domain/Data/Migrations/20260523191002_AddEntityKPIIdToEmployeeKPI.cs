using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityKPIIdToEmployeeKPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EntityKPIId",
                schema: "perf",
                table: "EmployeeKPIs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIs_EntityKPIId",
                schema: "perf",
                table: "EmployeeKPIs",
                column: "EntityKPIId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeKPIs_EntityKPIs_EntityKPIId",
                schema: "perf",
                table: "EmployeeKPIs",
                column: "EntityKPIId",
                principalSchema: "perf",
                principalTable: "EntityKPIs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeKPIs_EntityKPIs_EntityKPIId",
                schema: "perf",
                table: "EmployeeKPIs");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeKPIs_EntityKPIId",
                schema: "perf",
                table: "EmployeeKPIs");

            migrationBuilder.DropColumn(
                name: "EntityKPIId",
                schema: "perf",
                table: "EmployeeKPIs");
        }
    }
}
