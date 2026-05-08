using EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Services.Performance;

public interface IFormQuestionService
{
    Task<SuccessResponse> CreateAsync(CreateFormQuestionDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateFormQuestionDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse> GetByIdAsync(long id);
    Task<SuccessResponse> GetAllAsync();
    Task<SuccessResponse> GetByTemplateIdAsync(long templateId);
    Task<SuccessResponse> GetByCategoryIdAsync(long categoryId);
}