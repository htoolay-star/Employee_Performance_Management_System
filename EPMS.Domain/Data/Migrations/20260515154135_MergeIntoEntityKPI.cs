using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class MergeIntoEntityKPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityKPIs",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_EntityKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityKPIs_KPIMaster_KPIId",
                        column: x => x.KPIId,
                        principalSchema: "perf",
                        principalTable: "KPIMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityKPIs_KPIWeightPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "perf",
                        principalTable: "KPIWeightPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityKPIs_EntityType_EntityId_KPIId",
                schema: "perf",
                table: "EntityKPIs",
                columns: new[] { "EntityType", "EntityId", "KPIId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_EntityKPIs_KPIId",
                schema: "perf",
                table: "EntityKPIs",
                column: "KPIId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityKPIs_PriorityId",
                schema: "perf",
                table: "EntityKPIs",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityKPIs_PublicId",
                schema: "perf",
                table: "EntityKPIs",
                column: "PublicId",
                unique: true,
                filter: "[IsDeleted] = 0");

            // Migrate data from PositionKPIs
            migrationBuilder.Sql(@"
                INSERT INTO perf.EntityKPIs (EntityType, EntityId, KPIId, PriorityId, Weightage, TargetValue, TargetUnit, IsDeleted, DeletedAt, PublicId, CreatedAt, UpdatedAt)
                SELECT 'POSITION', PositionId, KPIId, PriorityId, Weightage, TargetValue, TargetUnit, IsDeleted, DeletedAt, PublicId, CreatedAt, UpdatedAt
                FROM perf.PositionKPIs");

            // Migrate data from DeptKPIs
            migrationBuilder.Sql(@"
                INSERT INTO perf.EntityKPIs (EntityType, EntityId, KPIId, PriorityId, Weightage, TargetValue, TargetUnit, IsDeleted, DeletedAt, PublicId, CreatedAt, UpdatedAt)
                SELECT 'DEPARTMENT', DeptId, KPIId, PriorityId, Weightage, TargetValue, TargetUnit, IsDeleted, DeletedAt, PublicId, CreatedAt, UpdatedAt
                FROM perf.DeptKPIs");

            // Migrate data from TeamKPIs
            migrationBuilder.Sql(@"
                INSERT INTO perf.EntityKPIs (EntityType, EntityId, KPIId, PriorityId, Weightage, TargetValue, TargetUnit, IsDeleted, DeletedAt, PublicId, CreatedAt, UpdatedAt)
                SELECT 'TEAM', TeamId, KPIId, PriorityId, Weightage, TargetValue, TargetUnit, IsDeleted, DeletedAt, PublicId, CreatedAt, UpdatedAt
                FROM perf.TeamKPIs");

            // Drop old tables after data migration
            migrationBuilder.DropTable(name: "DeptKPIs", schema: "perf");
            migrationBuilder.DropTable(name: "PositionKPIHistories", schema: "perf");
            migrationBuilder.DropTable(name: "PositionKPIs", schema: "perf");
            migrationBuilder.DropTable(name: "TeamKPIs", schema: "perf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityKPIs",
                schema: "perf");

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
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
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
                name: "PositionKPIHistories",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangedById = table.Column<long>(type: "bigint", nullable: true),
                    KPIId = table.Column<long>(type: "bigint", nullable: false),
                    PositionId = table.Column<long>(type: "bigint", nullable: false),
                    PriorityId = table.Column<long>(type: "bigint", nullable: false),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TargetUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TargetValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionKPIHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionKPIHistories_KPIMaster_KPIId",
                        column: x => x.KPIId,
                        principalSchema: "perf",
                        principalTable: "KPIMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PositionKPIHistories_KPIWeightPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "perf",
                        principalTable: "KPIWeightPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionKPIHistories_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "hr",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PositionKPIHistories_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PositionKPIs",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KPIId = table.Column<long>(type: "bigint", nullable: false),
                    PositionId = table.Column<long>(type: "bigint", nullable: false),
                    PriorityId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionKPIs_KPIMaster_KPIId",
                        column: x => x.KPIId,
                        principalSchema: "perf",
                        principalTable: "KPIMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PositionKPIs_KPIWeightPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "perf",
                        principalTable: "KPIWeightPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionKPIs_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "hr",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamKPIs",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KPIId = table.Column<long>(type: "bigint", nullable: false),
                    PriorityId = table.Column<long>(type: "bigint", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
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
                name: "IX_PositionKPIHistories_ChangedById",
                schema: "perf",
                table: "PositionKPIHistories",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_PositionKPIHistories_EndDate",
                schema: "perf",
                table: "PositionKPIHistories",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_PositionKPIHistories_KPIId_EffectiveDate",
                schema: "perf",
                table: "PositionKPIHistories",
                columns: new[] { "KPIId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PositionKPIHistories_PositionId_EffectiveDate",
                schema: "perf",
                table: "PositionKPIHistories",
                columns: new[] { "PositionId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PositionKPIHistories_PriorityId",
                schema: "perf",
                table: "PositionKPIHistories",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionKPIs_KPIId",
                schema: "perf",
                table: "PositionKPIs",
                column: "KPIId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionKPIs_PositionId_KPIId",
                schema: "perf",
                table: "PositionKPIs",
                columns: new[] { "PositionId", "KPIId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PositionKPIs_PriorityId",
                schema: "perf",
                table: "PositionKPIs",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionKPIs_PublicId",
                schema: "perf",
                table: "PositionKPIs",
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
    }
}
