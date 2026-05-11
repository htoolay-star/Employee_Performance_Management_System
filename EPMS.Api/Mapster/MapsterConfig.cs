using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Mapster;

namespace EPMS.Api.Mapster;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<EmployeeEmployment, EmployeeEmploymentDto>.NewConfig()
            .Map(dest => dest.DepartmentName, src => src.Department.Name)
            .Map(dest => dest.ParentDepartmentName, src => src.ParentDepartment.Name)
            .Map(dest => dest.TeamName, src => src.Team != null ? src.Team.Name : null)
            .Map(dest => dest.PositionTitle, src => src.Position.Name)
            .Map(dest => dest.DirectManagerName, src =>
                src.DirectManager != null
                    ? $"{src.DirectManager.FirstName} {src.DirectManager.LastName}"
                    : null);

        TypeAdapterConfig<EmployeeEmploymentHistory, EmployeeEmploymentHistoryDto>.NewConfig()
            .Map(dest => dest.DepartmentName, src => src.Department.Name)
            .Map(dest => dest.PositionTitle, src => src.Position.Name)
            .Map(dest => dest.ManagerName, src =>
                src.Manager != null
                    ? $"{src.Manager.FirstName} {src.Manager.LastName}"
                    : null)
            .Map(dest => dest.ChangedByName, src =>
                src.ChangedBy != null && src.ChangedBy.Profile != null
                    ? $"{src.ChangedBy.Profile.FirstName} {src.ChangedBy.Profile.LastName}"
                    : "System");

        TypeAdapterConfig<EmployeeSalaryHistory, EmployeeSalaryHistoryDto>.NewConfig()
            .Map(dest => dest.ApprovedByName, src =>
                src.ApprovedBy != null && src.ApprovedBy.Profile != null
                    ? $"{src.ApprovedBy.Profile.FirstName} {src.ApprovedBy.Profile.LastName}"
                    : "System");
    }
}
