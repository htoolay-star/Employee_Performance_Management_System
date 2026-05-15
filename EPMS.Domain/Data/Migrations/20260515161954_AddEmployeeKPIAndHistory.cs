using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeKPIAndHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeKPIHistories",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CycleId = table.Column<long>(type: "bigint", nullable: false),
                    KPIId = table.Column<long>(type: "bigint", nullable: false),
                    PriorityId = table.Column<long>(type: "bigint", nullable: false),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TargetValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SnapshotDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeKPIHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIHistories_AppraisalCycles_CycleId",
                        column: x => x.CycleId,
                        principalSchema: "perf",
                        principalTable: "AppraisalCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIHistories_KPIMaster_KPIId",
                        column: x => x.KPIId,
                        principalSchema: "perf",
                        principalTable: "KPIMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIHistories_KPIWeightPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "perf",
                        principalTable: "KPIWeightPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIHistories_CycleId",
                schema: "perf",
                table: "EmployeeKPIHistories",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIHistories_EmployeeId_CycleId",
                schema: "perf",
                table: "EmployeeKPIHistories",
                columns: new[] { "EmployeeId", "CycleId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIHistories_KPIId",
                schema: "perf",
                table: "EmployeeKPIHistories",
                column: "KPIId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIHistories_PriorityId",
                schema: "perf",
                table: "EmployeeKPIHistories",
                column: "PriorityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeKPIHistories",
                schema: "perf");
        }
    }
}
