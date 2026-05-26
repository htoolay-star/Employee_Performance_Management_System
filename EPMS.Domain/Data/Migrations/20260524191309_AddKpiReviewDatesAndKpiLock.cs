using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKpiReviewDatesAndKpiLock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "KpiLockIsDeadline",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "KpiLocked",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "KpiReviewDeadline",
                schema: "perf",
                table: "AppraisalCycles",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "KpiReviewStartDate",
                schema: "perf",
                table: "AppraisalCycles",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KpiLockIsDeadline",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "KpiLocked",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "KpiReviewDeadline",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "KpiReviewStartDate",
                schema: "perf",
                table: "AppraisalCycles");
        }
    }
}
