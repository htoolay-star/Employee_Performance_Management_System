using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleLockAndSubmittedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SubmittedAt",
                schema: "perf",
                table: "EvaluationResponses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ManagerLockIsDeadline",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ManagerLocked",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SelfLockIsDeadline",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SelfLocked",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ThreeSixtyLockIsDeadline",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ThreeSixtyLocked",
                schema: "perf",
                table: "Appraisals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                schema: "perf",
                table: "EvaluationResponses");

            migrationBuilder.DropColumn(
                name: "ManagerLockIsDeadline",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "ManagerLocked",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "SelfLockIsDeadline",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "SelfLocked",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "ThreeSixtyLockIsDeadline",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "ThreeSixtyLocked",
                schema: "perf",
                table: "Appraisals");
        }
    }
}
