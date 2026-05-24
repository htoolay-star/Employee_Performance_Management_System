using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePerformanceLevelFromRatingScale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerformanceLevel",
                schema: "hr",
                table: "RatingScales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PerformanceLevel",
                schema: "hr",
                table: "RatingScales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
