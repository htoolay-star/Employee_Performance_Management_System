using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
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
    }
}
