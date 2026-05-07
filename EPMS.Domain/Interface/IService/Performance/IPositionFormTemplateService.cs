using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PositionFormTemplateDTOs;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IPositionFormTemplateService
{
    Task<SuccessResponse<IEnumerable<PositionFormTemplateDto>>> GetAllAsync();
    Task<SuccessResponse<PositionFormTemplateDto>> GetByIdAsync(long id);
    Task<SuccessResponse<IEnumerable<PositionFormTemplateDto>>> GetByPositionIdAsync(long positionId);
    Task<SuccessResponse<long>> CreateAsync(CreatePositionFormTemplateDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdatePositionFormTemplateDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}