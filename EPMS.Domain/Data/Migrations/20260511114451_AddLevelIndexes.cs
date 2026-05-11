using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLevelIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Levels_IsActive",
                schema: "hr",
                table: "Levels",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_Name",
                schema: "hr",
                table: "Levels",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Levels_IsActive",
                schema: "hr",
                table: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Levels_Name",
                schema: "hr",
                table: "Levels");
        }
    }
}
