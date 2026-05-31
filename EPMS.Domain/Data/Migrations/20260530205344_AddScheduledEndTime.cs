using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduledEndTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ScheduledEndTime",
                schema: "perf",
                table: "OneOnOneMeetings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledEndTime",
                schema: "perf",
                table: "OneOnOneMeetings");
        }
    }
}
