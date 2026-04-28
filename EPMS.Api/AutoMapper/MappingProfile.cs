using AutoMapper;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.HR;
using EPMS.Shared.PositionDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EPMS.Api.MappingProfiles;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        
        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<Team, TeamDto>().ReverseMap();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<CreateTeamDto, Team>();

        CreateMap<CreatePositionDto, Position>();
        CreateMap<UpdatePositionDto, Position>();
    }

}