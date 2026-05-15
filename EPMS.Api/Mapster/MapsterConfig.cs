using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.Performance.DeptKPI;
using EPMS.Shared.DTOs.Performance.PositionKPI;
using EPMS.Shared.DTOs.Performance.TeamKPI;
using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs;
using EPMS.Shared.DTOs.TeamDTOs;
using Mapster;

namespace EPMS.Api.Mapster;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<EmployeeEmploymentHistory, EmployeeEmploymentHistoryDto>.NewConfig()
            .Map(dest => dest.PositionTitle, src => src.Position.Name)
            .Map(dest => dest.ChangedByName, src => src.ChangedBy.Profile.StaffName ?? "System");

        TypeAdapterConfig<EmployeeSalaryHistory, EmployeeSalaryHistoryDto>.NewConfig()
            .Map(dest => dest.ApprovedByName, src => src.ApprovedBy.Profile.StaffName ?? "System");

        TypeAdapterConfig<EmployeeProfile, EmployeeProfileGridItemDto>.NewConfig()
            .Map(dest => dest.DepartmentName, src => src.Employment.Department.Name)
            .Map(dest => dest.PositionName, src => src.Employment.Position.Name)
            .Map(dest => dest.TeamName, src => src.Employment.Team.Name)
            .Map(dest => dest.EmploymentStatus, src => src.Employment.EmploymentStatus);

        TypeAdapterConfig<Department, DepartmentDto>.NewConfig()
            .Map(dest => dest.DeptHeadName, src => src.DeptHead != null ? src.DeptHead.StaffName : null);

        TypeAdapterConfig<Team, TeamGridItemDto>.NewConfig()
            .Map(dest => dest.LeadTeamName, src => src.LeadTeam != null ? src.LeadTeam.StaffName : null);

        // Performance KPIs
        TypeAdapterConfig<PositionKPI, PositionKPIDto>.NewConfig()
            .Map(dest => dest.KPIName, src => src.KPI != null ? src.KPI.Name : string.Empty)
            .Map(dest => dest.KPICode, src => src.KPI != null ? src.KPI.Code : string.Empty)
            .Map(dest => dest.PriorityName, src => src.Priority != null ? src.Priority.LevelName : string.Empty);

        TypeAdapterConfig<DeptKPI, DeptKPIDto>.NewConfig()
            .Map(dest => dest.KPIName, src => src.KPI != null ? src.KPI.Name : string.Empty)
            .Map(dest => dest.KPICode, src => src.KPI != null ? src.KPI.Code : string.Empty)
            .Map(dest => dest.PriorityName, src => src.Priority != null ? src.Priority.LevelName : string.Empty);

        TypeAdapterConfig<TeamKPI, TeamKPIDto>.NewConfig()
            .Map(dest => dest.KPIName, src => src.KPI != null ? src.KPI.Name : string.Empty)
            .Map(dest => dest.KPICode, src => src.KPI != null ? src.KPI.Code : string.Empty)
            .Map(dest => dest.PriorityName, src => src.Priority != null ? src.Priority.LevelName : string.Empty);

        TypeAdapterConfig<KPIMaster, KPIMasterDto>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : string.Empty);

        // Performance - PIP
        TypeAdapterConfig<PIP, PIPDto>.NewConfig()
            .Map(dest => dest.EmployeeName, src => src.Employee != null ? src.Employee.StaffName : string.Empty)
            .Map(dest => dest.ManagerName, src => src.Manager != null ? src.Manager.StaffName : string.Empty);

        // Performance - Appraisal
        TypeAdapterConfig<Appraisal, AppraisalDto>.NewConfig()
            .Map(dest => dest.EmployeeName, src => src.Employee != null ? src.Employee.StaffName : null)
            .Map(dest => dest.CycleName, src => src.Cycle != null ? src.Cycle.Name : null)
            .Map(dest => dest.AppraiserName, src => src.Appraiser != null ? src.Appraiser.StaffName : null);

        TypeAdapterConfig<AppraisalRecommendation, AppraisalRecommendationDto>.NewConfig()
            .Map(dest => dest.AppraisalEmployeeName, src => src.Appraisal != null && src.Appraisal.Employee != null ? src.Appraisal.Employee.StaffName : null)
            .Map(dest => dest.ProcessedByName, src => src.ProcessedBy != null ? src.ProcessedBy.StaffName : null);

        TypeAdapterConfig<EvaluationResponse, EvaluationResponseDto>.NewConfig()
            .Map(dest => dest.AppraisalEmployeeName, src => src.Appraisal != null && src.Appraisal.Employee != null ? src.Appraisal.Employee.StaffName : null)
            .Map(dest => dest.TemplateName, src => src.Template != null ? src.Template.Name : null)
            .Map(dest => dest.QuestionText, src => src.Question != null ? src.Question.QuestionText : null)
            .Map(dest => dest.EvaluatorName, src => src.Evaluator != null ? src.Evaluator.StaffName : null);

        // Performance - Feedback & Meetings
        TypeAdapterConfig<ContinuousFeedback, ContinuousFeedbackDto>.NewConfig()
            .Map(dest => dest.EmployeeName, src => src.Employee != null ? src.Employee.StaffName : string.Empty)
            .Map(dest => dest.GivenByName, src => src.GivenBy != null ? src.GivenBy.StaffName : string.Empty);

        TypeAdapterConfig<OneOnOneMeeting, OneOnOneMeetingDto>.NewConfig()
            .Map(dest => dest.EmployeeName, src => src.Employee != null ? src.Employee.StaffName : string.Empty)
            .Map(dest => dest.ManagerName, src => src.Manager != null ? src.Manager.StaffName : string.Empty);

        // Auth
        TypeAdapterConfig<PositionPermission, PositionPermissionDto>.NewConfig()
            .Map(dest => dest.PositionTitle, src => src.Position != null ? src.Position.Name : null)
            .Map(dest => dest.PermissionName, src => src.Permission != null ? src.Permission.Name : null)
            .Map(dest => dest.PermissionCode, src => src.Permission != null ? src.Permission.Code : null);

        // EmployeeInfo - Employment with navigation flattening
        TypeAdapterConfig<EmployeeEmployment, EmployeeEmploymentDto>.NewConfig()
            .Map(dest => dest.DepartmentName, src => src.Department.Name)
            .Map(dest => dest.ParentDepartmentName, src => src.ParentDepartment.Name)
            .Map(dest => dest.TeamName, src => src.Team != null ? src.Team.Name : null)
            .Map(dest => dest.PositionName, src => src.Position.Name)
            .Map(dest => dest.DirectManagerName, src => src.DirectManager != null ? src.DirectManager.StaffName : null);

        // Performance - FormQuestion
        TypeAdapterConfig<FormQuestion, FormQuestionDto>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null)
            .Map(dest => dest.RatingScaleName, src => src.RatingScale != null ? src.RatingScale.Name : null);
    }
}
