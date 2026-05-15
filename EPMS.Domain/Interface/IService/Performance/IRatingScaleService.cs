using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IRatingScaleService
{
    Task<SuccessResponse<IEnumerable<RatingScaleDto>>> GetAllAsync();
    Task<SuccessResponse<IEnumerable<RatingScaleDto>>> GetActiveAsync();
    Task<SuccessResponse<RatingScaleDto>> GetByIdAsync(long id);
    Task<SuccessResponse<RatingScaleDto>> GetByRatingAsync(int rating);
    Task<SuccessResponse<long>> CreateAsync(CreateRatingScaleDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateRatingScaleDto dto);
    Task<SuccessResponse> RestoreAsync(long id);
        Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse> DeactivateAsync(long id);
    Task<SuccessResponse> ReactivateAsync(long id);
}
