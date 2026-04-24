using AutoMapper;
using EPMS.Domain.Common;
using EPMS.Domain.Entities.PerformanceReview;
using EPMS.Shared.DTOs;

namespace EPMS.Api.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            // Entity → DTO
            CreateMap<PerformanceReview, PerformanceReviewDto>();

            // DTO → Entity
            CreateMap<CreatePerformanceReivewDto, PerformanceReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewDate, opt => opt.Ignore())
                .ForMember(dest => dest.PerformanceLevel, opt => opt.Ignore())
                .ForMember(dest => dest.PromotionEligibility, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var (level, promotion) = PerformanceMapper.Map(src.Rating);
                    dest.PerformanceLevel = level;
                    dest.PromotionEligibility = promotion;
                    dest.ReviewDate = DateTime.UtcNow;
                    dest.Id = Guid.NewGuid();
                });
        }
    }
}
