using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamePeerScoreToThreeSixtyScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeerScore",
                schema: "perf",
                table: "Appraisals",
                newName: "ThreeSixtyScore");

            migrationBuilder.RenameColumn(
                name: "PeerWeight",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "ThreeSixtyWeight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThreeSixtyScore",
                schema: "perf",
                table: "Appraisals",
                newName: "PeerScore");

            migrationBuilder.RenameColumn(
                name: "ThreeSixtyWeight",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "PeerWeight");
        }
    }
}
