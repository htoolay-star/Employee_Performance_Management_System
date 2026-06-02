using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeptKPIAndTeamKPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeptKPIs",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeptId = table.Column<long>(type: "bigint", nullable: false),
                    KPIId = table.Column<long>(type: "bigint", nullable: false),
                    PriorityId = table.Column<long>(type: "bigint", nullable: false),
                    TargetValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeptKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeptKPIs_Departments_DeptId",
                        column: x => x.DeptId,
                        principalSchema: "hr",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeptKPIs_KPIMaster_KPIId",
                        column: x => x.KPIId,
                        principalSchema: "perf",
                        principalTable: "KPIMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeptKPIs_KPIWeightPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "perf",
                        principalTable: "KPIWeightPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamKPIs",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    KPIId = table.Column<long>(type: "bigint", nullable: false),
                    PriorityId = table.Column<long>(type: "bigint", nullable: false),
                    TargetValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamKPIs_KPIMaster_KPIId",
                        column: x => x.KPIId,
                        principalSchema: "perf",
                        principalTable: "KPIMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamKPIs_KPIWeightPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "perf",
                        principalTable: "KPIWeightPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamKPIs_Teams_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "hr",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeptKPIs_DeptId_KPIId",
                schema: "perf",
                table: "DeptKPIs",
                columns: new[] { "DeptId", "KPIId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_DeptKPIs_KPIId",
                schema: "perf",
                table: "DeptKPIs",
                column: "KPIId");

            migrationBuilder.CreateIndex(
                name: "IX_DeptKPIs_PriorityId",
                schema: "perf",
                table: "DeptKPIs",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_DeptKPIs_PublicId",
                schema: "perf",
                table: "DeptKPIs",
                column: "PublicId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamKPIs_KPIId",
                schema: "perf",
                table: "TeamKPIs",
                column: "KPIId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamKPIs_PriorityId",
                schema: "perf",
                table: "TeamKPIs",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamKPIs_PublicId",
                schema: "perf",
                table: "TeamKPIs",
                column: "PublicId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamKPIs_TeamId_KPIId",
                schema: "perf",
                table: "TeamKPIs",
                columns: new[] { "TeamId", "KPIId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeptKPIs",
                schema: "perf");

            migrationBuilder.DropTable(
                name: "TeamKPIs",
                schema: "perf");
        }
    }
}
