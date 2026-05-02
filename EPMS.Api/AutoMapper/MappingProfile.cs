using AutoMapper;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Shared;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.LevelDTOs;
using EPMS.Shared.DTOs.TagDTOs;
using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.DTOs.TeamDTOs;

namespace EPMS.Api.MappingProfiles;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        
        CreateMap<Department, DepartmentDto>();
        CreateMap<Level, LevelDto>();
        CreateMap<Team, TeamDto>();
        CreateMap<Permission, PermissionDto>();
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

        CreateMap<Category, CategoryDto>();
        CreateMap<Tag, TagDto>();

        CreateMap<Position, PositionDto>()
            .ForMember(dest => dest.LevelCode, opt => opt.MapFrom(src => src.Level.Code))
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.Name));
    }

}