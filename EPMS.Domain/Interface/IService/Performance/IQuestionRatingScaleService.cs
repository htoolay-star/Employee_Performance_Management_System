using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IQuestionRatingScaleService
{
    Task<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>> GetAllAsync();
    Task<SuccessResponse<QuestionRatingScaleDto>> GetByIdAsync(long id);
    Task<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>> GetActiveAsync();
    Task<SuccessResponse<long>> CreateAsync(CreateQuestionRatingScaleDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateQuestionRatingScaleDto dto);
    Task<SuccessResponse> RestoreAsync(long id);
        Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse> DeactivateAsync(long id);
    Task<SuccessResponse> ReactivateAsync(long id);
}