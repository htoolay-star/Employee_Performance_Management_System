using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamePeerReviewToThreeSixtyReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeerReviewStartDate",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "ThreeSixtyReviewStartDate");

            migrationBuilder.RenameColumn(
                name: "PeerReviewDeadline",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "ThreeSixtyReviewDeadline");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThreeSixtyReviewStartDate",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "PeerReviewStartDate");

            migrationBuilder.RenameColumn(
                name: "ThreeSixtyReviewDeadline",
                schema: "perf",
                table: "AppraisalCycles",
                newName: "PeerReviewDeadline");
        }
    }
}
