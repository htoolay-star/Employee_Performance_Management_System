using AutoMapper;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.DTOs.Form;

namespace EPMS.Application.Mappings
{
    public class AppraisalProfile : Profile
    {
        public AppraisalProfile()
        {
            // Request DTO to Entity
            CreateMap<AppraisalSubmissionDto, Appraisal>()
                // Note: DTO uses 'EvaluatorId' but your Entity uses 'AppraiserId'
                .ForMember(dest => dest.AppraiserId, opt => opt.MapFrom(src => src.EvaluatorId))
                .ForMember(dest => dest.EvaluatorRole, opt => opt.MapFrom(src => src.EvaluatorRole))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Completed"))
                .ForMember(dest => dest.Details, opt => opt.Ignore()); // Details are handled separately in service

            // Entity to Response DTO
            CreateMap<Appraisal, AppraisalResponseDto>()
                .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore ?? 0))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.RatingLabel ?? "Pending"));
        }
    }
}