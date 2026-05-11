using AutoMapper;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Shared;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.DTOs.LevelDTOs;
using EPMS.Shared.DTOs.TagDTOs;
using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;
using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;
using EPMS.Shared.DTOs.Performance.PositionKPI;
using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;

namespace EPMS.Api.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Auth Entities
        CreateMap<Permission, PermissionDto>();
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

        // Shared Entities
        CreateMap<Category, CategoryDto>();
        CreateMap<Tag, TagDto>();

        // EmployeeInfo Entities
        CreateMap<EmployeeProfile, EmployeeProfileDto>();
        CreateMap<EmployeeProfile, EmployeeProfileDetailDto>();
        
        CreateMap<EmployeeEmployment, EmployeeEmploymentDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.ParentDepartmentName, opt => opt.MapFrom(src => src.ParentDepartment.Name))
            .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : null))
            .ForMember(dest => dest.PositionTitle, opt => opt.MapFrom(src => src.Position.Title))
            .ForMember(dest => dest.DirectManagerName, opt => opt.MapFrom(src => src.DirectManager != null ? src.DirectManager.FirstName + " " + src.DirectManager.LastName : null));

        CreateMap<EmployeeContact, EmployeeContactDto>();
        CreateMap<EmployeeFamilyInfo, EmployeeFamilyInfoDto>();
        
        CreateMap<EmployeePayrollInfo, EmployeePayrollInfoDto>();
        
        CreateMap<EmployeeEmploymentHistory, EmployeeEmploymentHistoryDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.PositionTitle, opt => opt.MapFrom(src => src.Position.Title))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FirstName + " " + src.Manager.LastName : null))
            .ForMember(dest => dest.ChangedByName, opt => opt.MapFrom(src =>
                src.ChangedBy != null && src.ChangedBy.Profile != null
                ? $"{src.ChangedBy.Profile.FirstName} {src.ChangedBy.Profile.LastName}"
                : "System"));

        CreateMap<EmployeeSalaryHistory, EmployeeSalaryHistoryDto>()
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src =>
                src.ApprovedBy != null && src.ApprovedBy.Profile != null
                ? $"{src.ApprovedBy.Profile.FirstName} {src.ApprovedBy.Profile.LastName}"
                : "System"));

        // Performance Entities
        CreateMap<RatingScale, RatingScaleDto>();
        CreateMap<KPIWeightPriority, KPIWeightPriorityDto>();
        CreateMap<AppraisalCycle, AppraisalCycleDto>();
        CreateMap<KPIMaster, KPIMasterDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

        CreateMap<PIP, PIPDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? $"{src.Employee.FirstName} {src.Employee.LastName}" : string.Empty))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? $"{src.Manager.FirstName} {src.Manager.LastName}" : string.Empty));

        CreateMap<FormTemplate, FormTemplateDto>();

        CreateMap<FormQuestion, FormQuestionDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.RatingScaleName, opt => opt.MapFrom(src => src.RatingScale != null ? src.RatingScale.Name : null))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()));

        CreateMap<ContinuousFeedback, ContinuousFeedbackDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? $"{src.Employee.FirstName} {src.Employee.LastName}" : string.Empty))
            .ForMember(dest => dest.GivenByName, opt => opt.MapFrom(src => src.GivenBy != null ? $"{src.GivenBy.FirstName} {src.GivenBy.LastName}" : string.Empty));

        CreateMap<OneOnOneMeeting, OneOnOneMeetingDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? $"{src.Employee.FirstName} {src.Employee.LastName}" : string.Empty))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? $"{src.Manager.FirstName} {src.Manager.LastName}" : string.Empty));

        CreateMap<PositionKPI, PositionKPIDto>()
            .ForMember(dest => dest.KPIName, opt => opt.MapFrom(src => src.KPI != null ? src.KPI.Name : string.Empty))
            .ForMember(dest => dest.KPICode, opt => opt.MapFrom(src => src.KPI != null ? src.KPI.Code : string.Empty))
            .ForMember(dest => dest.PriorityName, opt => opt.MapFrom(src => src.Priority != null ? src.Priority.LevelName : string.Empty));

        CreateMap<PositionPermission, PositionPermissionDto>()
            .ForMember(dest => dest.PositionTitle, opt => opt.MapFrom(src => src.Position != null ? src.Position.Title : null))
            .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission != null ? src.Permission.Name : null))
            .ForMember(dest => dest.PermissionCode, opt => opt.MapFrom(src => src.Permission != null ? src.Permission.Code : null));

        // Form DTOs - Appraisal Mappings
        CreateMap<Appraisal, AppraisalDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => 
                src.Employee != null ? $"{src.Employee.FirstName} {src.Employee.LastName}" : null))
            .ForMember(dest => dest.CycleName, opt => opt.MapFrom(src => src.Cycle != null ? src.Cycle.Name : null))
            .ForMember(dest => dest.AppraiserName, opt => opt.MapFrom(src => 
                src.Appraiser != null ? $"{src.Appraiser.FirstName} {src.Appraiser.LastName}" : null));

        CreateMap<AppraisalSubmissionDto, Appraisal>()
            .ForMember(dest => dest.AppraiserId, opt => opt.MapFrom(src => src.EvaluatorId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Completed"))
            .ForMember(dest => dest.Details, opt => opt.Ignore());

        CreateMap<Appraisal, AppraisalResponseDto>()
            .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore ?? 0))
            .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.RatingLabel ?? "Pending"));

        // Form DTOs - AppraisalRecommendation Mappings
        CreateMap<AppraisalRecommendation, AppraisalRecommendationDto>()
            .ForMember(dest => dest.AppraisalEmployeeName, opt => opt.MapFrom(src => 
                src.Appraisal != null && src.Appraisal.Employee != null ? $"{src.Appraisal.Employee.FirstName} {src.Appraisal.Employee.LastName}" : null))
            .ForMember(dest => dest.ProcessedByName, opt => opt.MapFrom(src => 
                src.ProcessedBy != null ? $"{src.ProcessedBy.FirstName} {src.ProcessedBy.LastName}" : null));

        // Form DTOs - EvaluationResponse Mappings
        CreateMap<EvaluationResponse, EvaluationResponseDto>()
            .ForMember(dest => dest.AppraisalEmployeeName, opt => opt.MapFrom(src => 
                src.Appraisal != null && src.Appraisal.Employee != null ? $"{src.Appraisal.Employee.FirstName} {src.Appraisal.Employee.LastName}" : null))
            .ForMember(dest => dest.TemplateName, opt => opt.MapFrom(src => src.Template != null ? src.Template.Name : null))
            .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question != null ? src.Question.QuestionText : null))
            .ForMember(dest => dest.EvaluatorName, opt => opt.MapFrom(src => 
                src.Evaluator != null ? $"{src.Evaluator.FirstName} {src.Evaluator.LastName}" : null));
    }
}