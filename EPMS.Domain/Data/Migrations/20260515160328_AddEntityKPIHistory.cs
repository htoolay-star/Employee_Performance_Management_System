using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityKPIHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityKPIHistories",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_EntityKPIHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityKPIHistories_AppraisalCycles_CycleId",
                        column: x => x.CycleId,
                        principalSchema: "perf",
                        principalTable: "AppraisalCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityKPIHistories_KPIMaster_KPIId",
                        column: x => x.KPIId,
                        principalSchema: "perf",
                        principalTable: "KPIMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityKPIHistories_KPIWeightPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "perf",
                        principalTable: "KPIWeightPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityKPIHistories_CycleId",
                schema: "perf",
                table: "EntityKPIHistories",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityKPIHistories_EntityType_EntityId_CycleId",
                schema: "perf",
                table: "EntityKPIHistories",
                columns: new[] { "EntityType", "EntityId", "CycleId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityKPIHistories_KPIId",
                schema: "perf",
                table: "EntityKPIHistories",
                column: "KPIId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityKPIHistories_PriorityId",
                schema: "perf",
                table: "EntityKPIHistories",
                column: "PriorityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityKPIHistories",
                schema: "perf");
        }
    }
}
