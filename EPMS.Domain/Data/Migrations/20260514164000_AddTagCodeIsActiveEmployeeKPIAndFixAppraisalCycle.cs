using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTagCodeIsActiveEmployeeKPIAndFixAppraisalCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppraisalCycles_Name_Year",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "Year",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "WindowStartDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "WindowEndDate");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "shared",
                table: "Tags",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "shared",
                table: "Tags",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "shared",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<long>(
                name: "EmployeeKPIId",
                schema: "perf",
                table: "AppraisalDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalendarType",
                schema: "perf",
                table: "AppraisalCycles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EvaluationEndDate",
                schema: "perf",
                table: "AppraisalCycles",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "EvaluationStartDate",
                schema: "perf",
                table: "AppraisalCycles",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "YearLabel",
                schema: "perf",
                table: "AppraisalCycles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EmployeeKPIs",
                schema: "perf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    KPIId = table.Column<long>(type: "bigint", nullable: false),
                    CycleId = table.Column<long>(type: "bigint", nullable: false),
                    PriorityId = table.Column<long>(type: "bigint", nullable: false),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TargetValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIs_AppraisalCycles_CycleId",
                        column: x => x.CycleId,
                        principalSchema: "perf",
                        principalTable: "AppraisalCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIs_EmployeeProfiles_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIs_KPIMaster_KPIId",
                        column: x => x.KPIId,
                        principalSchema: "perf",
                        principalTable: "KPIMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIs_KPIWeightPriorities_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "perf",
                        principalTable: "KPIWeightPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Categories_IsActive",
                schema: "shared",
                table: "Categories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                schema: "shared",
                table: "Categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalCycles_Name_YearLabel",
                schema: "perf",
                table: "AppraisalCycles",
                columns: new[] { "Name", "YearLabel" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIs_CycleId",
                schema: "perf",
                table: "EmployeeKPIs",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIs_EmployeeId_KPIId_CycleId",
                schema: "perf",
                table: "EmployeeKPIs",
                columns: new[] { "EmployeeId", "KPIId", "CycleId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIs_KPIId",
                schema: "perf",
                table: "EmployeeKPIs",
                column: "KPIId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIs_PriorityId",
                schema: "perf",
                table: "EmployeeKPIs",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIs_PublicId",
                schema: "perf",
                table: "EmployeeKPIs",
                column: "PublicId",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeKPIs",
                schema: "perf");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Code",
                schema: "shared",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_IsActive",
                schema: "shared",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IsActive",
                schema: "shared",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                schema: "shared",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AppraisalCycles_Name_YearLabel",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "shared",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "shared",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "EmployeeKPIId",
                schema: "perf",
                table: "AppraisalDetails");

            migrationBuilder.DropColumn(
                name: "CalendarType",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "EvaluationEndDate",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "EvaluationStartDate",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "YearLabel",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.RenameColumn(
                name: "WindowStartDate",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "WindowEndDate",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "EndDate");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "shared",
                table: "Tags",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                schema: "perf",
                table: "AppraisalCycles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalCycles_Name_Year",
                schema: "perf",
                table: "AppraisalCycles",
                columns: new[] { "Name", "Year" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
