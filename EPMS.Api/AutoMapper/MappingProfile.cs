using AutoMapper;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Shared;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.HR;
using EPMS.Shared.DTOs.SharedDTOs.CategoryDTOs;
using EPMS.Shared.DTOs.SharedDTOs.TagDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EPMS.Api.MappingProfiles;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        
        CreateMap<Department, DepartmentDto>();
        CreateMap<Team, TeamDto>();
        CreateMap<Permission, PermissionDto>();
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

        CreateMap<CreatePermissionDto, Permission>()
                .ConstructUsing(src => new Permission(src.Code, src.Name, src.Description));

        CreateMap<Category, CategoryDto>();
        CreateMap<Tag, TagDto>();
    }

}