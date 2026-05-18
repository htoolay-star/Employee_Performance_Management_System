using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs;

namespace EPMS.Domain.Interface.IService.Performance
{
    public interface IFormTemplateService
    {
        Task<SuccessResponse<IEnumerable<FormTemplateDto>>> GetAllAsync();
        Task<SuccessResponse<IEnumerable<FormTemplateDto>>> GetActiveAsync();
        Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();
        Task<SuccessResponse<FormTemplateDto>> GetByIdAsync(long id);
        Task<SuccessResponse<long>> CreateAsync(CreateFormTemplateDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateFormTemplateDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
        Task<SuccessResponse<FormTemplatePreviewDto>> GetPreviewAsync(long templateId);
    }
}