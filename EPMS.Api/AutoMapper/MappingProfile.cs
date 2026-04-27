using AutoMapper;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.HR;
using EPMS.Shared.PermissionDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EPMS.Api.MappingProfiles;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        
        CreateMap<Department, DepartmentDto>();
        CreateMap<Team, TeamDto>();
        CreateMap<Permission, PermissionDto>();

        CreateMap<CreatePermissionDto, Permission>()
                .ConstructUsing(src => new Permission(src.Code, src.Name, src.Description));
    }

}