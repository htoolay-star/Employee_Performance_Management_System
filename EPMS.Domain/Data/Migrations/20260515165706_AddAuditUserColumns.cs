using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditUserColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "auth",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "auth",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "auth",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "Teams",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "Teams",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "Teams",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "app",
                table: "SystemSettings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "app",
                table: "SystemSettings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "app",
                table: "SystemSettings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "auth",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "auth",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "auth",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "RatingScales",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "RatingScales",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "RatingScales",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "QuestionRatingScales",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "QuestionRatingScales",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "QuestionRatingScales",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "auth",
                table: "PositionRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "auth",
                table: "PositionRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "auth",
                table: "PositionRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "PositionPIPTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "PositionPIPTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "PositionPIPTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "auth",
                table: "PositionPermissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "auth",
                table: "PositionPermissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "auth",
                table: "PositionPermissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "PositionFormTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "PositionFormTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "PositionFormTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "PIPs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "PIPs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "PIPs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "PIPObjectives",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "PIPObjectives",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "PIPObjectives",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "auth",
                table: "Permissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "auth",
                table: "Permissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "auth",
                table: "Permissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "OneOnOneMeetings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "OneOnOneMeetings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "OneOnOneMeetings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "Levels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "Levels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "Levels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "KPIWeightPriorities",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "KPIWeightPriorities",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "KPIWeightPriorities",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "KPIMaster",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "KPIMaster",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "KPIMaster",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "FormTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "FormTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "FormTemplates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "FormQuestions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "FormQuestions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "FormQuestions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "EvaluationResponses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "EvaluationResponses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "EvaluationResponses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "EntityKPIs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "EntityKPIs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "EntityKPIs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeeProfiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeeProfiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeeProfiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeePayrollInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeePayrollInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeePayrollInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "EmployeeKPIs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "EmployeeKPIs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "EmployeeKPIs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeeFamilyInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeeFamilyInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeeFamilyInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeeEmployment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeeEmployment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeeEmployment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeeContacts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeeContacts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeeContacts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "shared",
                table: "DocumentAttachments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "shared",
                table: "DocumentAttachments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "shared",
                table: "DocumentAttachments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "hr",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "hr",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "hr",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "ContinuousFeedbacks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "ContinuousFeedbacks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "ContinuousFeedbacks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "shared",
                table: "Categories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "shared",
                table: "Categories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "shared",
                table: "Categories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "Appraisals",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "Appraisals",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "Appraisals",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "AppraisalRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "AppraisalRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "AppraisalRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "AppraisalDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "AppraisalDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "AppraisalDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "perf",
                table: "AppraisalCycles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                schema: "perf",
                table: "AppraisalCycles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                schema: "perf",
                table: "AppraisalCycles",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "app",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "app",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "app",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "RatingScales");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "RatingScales");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "RatingScales");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "QuestionRatingScales");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "QuestionRatingScales");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "QuestionRatingScales");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "PositionRoles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "auth",
                table: "PositionRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "auth",
                table: "PositionRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "PositionPIPTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "PositionPIPTemplates");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "PositionPIPTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "PositionPermissions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "auth",
                table: "PositionPermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "auth",
                table: "PositionPermissions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "PositionFormTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "PositionFormTemplates");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "PositionFormTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "PIPs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "PIPs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "PIPs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "PIPObjectives");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "PIPObjectives");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "PIPObjectives");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "auth",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "auth",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "OneOnOneMeetings");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "OneOnOneMeetings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "OneOnOneMeetings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "KPIWeightPriorities");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "KPIWeightPriorities");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "KPIWeightPriorities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "KPIMaster");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "KPIMaster");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "KPIMaster");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "FormTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "FormTemplates");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "FormTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "FormQuestions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "FormQuestions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "FormQuestions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "EvaluationResponses");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "EvaluationResponses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "EvaluationResponses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "EntityKPIs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "EntityKPIs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "EntityKPIs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeePayrollInfo");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeePayrollInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeePayrollInfo");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "EmployeeKPIs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "EmployeeKPIs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "EmployeeKPIs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeeFamilyInfo");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeeFamilyInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeeFamilyInfo");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeeEmployment");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeeEmployment");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeeEmployment");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "EmployeeContacts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "EmployeeContacts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "EmployeeContacts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "shared",
                table: "DocumentAttachments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "shared",
                table: "DocumentAttachments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "shared",
                table: "DocumentAttachments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "hr",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "hr",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "ContinuousFeedbacks");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "ContinuousFeedbacks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "ContinuousFeedbacks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "shared",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "shared",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "shared",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "Appraisals");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "AppraisalRecommendations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "AppraisalRecommendations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "AppraisalRecommendations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "AppraisalDetails");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "AppraisalDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "AppraisalDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "perf",
                table: "AppraisalCycles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "perf",
                table: "AppraisalCycles");
        }
    }
}
