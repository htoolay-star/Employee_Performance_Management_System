using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeDescriptionAndFKToHrEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "hr",
                table: "Positions",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_Title",
                schema: "hr",
                table: "Positions",
                newName: "IX_Positions_Name");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "hr",
                table: "Teams",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "hr",
                table: "Teams",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LeadTeamId",
                schema: "hr",
                table: "Teams",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "hr",
                table: "Positions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "hr",
                table: "Positions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeptHeadId",
                schema: "hr",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "hr",
                table: "Departments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Code",
                schema: "hr",
                table: "Teams",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_IsActive",
                schema: "hr",
                table: "Teams",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeadTeamId",
                schema: "hr",
                table: "Teams",
                column: "LeadTeamId",
                unique: true,
                filter: "[LeadTeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Code",
                schema: "hr",
                table: "Positions",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_IsActive",
                schema: "hr",
                table: "Positions",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DeptHeadId",
                schema: "hr",
                table: "Departments",
                column: "DeptHeadId",
                unique: true,
                filter: "[DeptHeadId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_IsActive",
                schema: "hr",
                table: "Departments",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_EmployeeProfiles_DeptHeadId",
                schema: "hr",
                table: "Departments",
                column: "DeptHeadId",
                principalSchema: "hr",
                principalTable: "EmployeeProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_EmployeeProfiles_LeadTeamId",
                schema: "hr",
                table: "Teams",
                column: "LeadTeamId",
                principalSchema: "hr",
                principalTable: "EmployeeProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_EmployeeProfiles_DeptHeadId",
                schema: "hr",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_EmployeeProfiles_LeadTeamId",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_Code",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_IsActive",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LeadTeamId",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Positions_Code",
                schema: "hr",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_IsActive",
                schema: "hr",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Departments_DeptHeadId",
                schema: "hr",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_IsActive",
                schema: "hr",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LeadTeamId",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "hr",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "hr",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "DeptHeadId",
                schema: "hr",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "hr",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "hr",
                table: "Positions",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_Name",
                schema: "hr",
                table: "Positions",
                newName: "IX_Positions_Title");
        }
    }
}
