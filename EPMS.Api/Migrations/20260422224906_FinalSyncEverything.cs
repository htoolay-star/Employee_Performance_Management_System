using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class FinalSyncEverything : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Departments_DepartmentId1",
                schema: "hr",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "Teams",
                schema: "hr",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "Positions",
                schema: "hr",
                newName: "Positions");

            migrationBuilder.RenameTable(
                name: "Levels",
                schema: "hr",
                newName: "Levels");

            migrationBuilder.RenameTable(
                name: "Departments",
                schema: "hr",
                newName: "Departments");

            migrationBuilder.RenameColumn(
                name: "DepartmentId1",
                table: "Teams",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_DepartmentId1",
                table: "Teams",
                newName: "IX_Teams_TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Teams_TeamId",
                table: "Teams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Teams_TeamId",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Teams",
                newSchema: "hr");

            migrationBuilder.RenameTable(
                name: "Positions",
                newName: "Positions",
                newSchema: "hr");

            migrationBuilder.RenameTable(
                name: "Levels",
                newName: "Levels",
                newSchema: "hr");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Departments",
                newSchema: "hr");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                schema: "hr",
                table: "Teams",
                newName: "DepartmentId1");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_TeamId",
                schema: "hr",
                table: "Teams",
                newName: "IX_Teams_DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Departments_DepartmentId1",
                schema: "hr",
                table: "Teams",
                column: "DepartmentId1",
                principalSchema: "hr",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
