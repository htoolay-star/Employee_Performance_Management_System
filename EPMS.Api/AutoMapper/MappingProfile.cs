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

namespace EPMS.Api.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // HR Entities
        CreateMap<Department, DepartmentDto>();
        CreateMap<Level, LevelDto>();
        CreateMap<Team, TeamDto>();
        CreateMap<Position, PositionDto>()
            .ForMember(dest => dest.LevelCode, opt => opt.MapFrom(src => src.Level.Code))
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.Name));

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
    }
}