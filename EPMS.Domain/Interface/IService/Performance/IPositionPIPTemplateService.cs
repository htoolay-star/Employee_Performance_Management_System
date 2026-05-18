using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IPositionPIPTemplateService
{
    Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetAllAsync();
    Task<SuccessResponse<PositionPIPTemplateDto>> GetByIdAsync(long id);
    Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetByPositionIdAsync(long positionId);
    Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetActiveByPositionIdAsync(long positionId);
    Task<SuccessResponse<long>> CreateAsync(CreatePositionPIPTemplateDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdatePositionPIPTemplateDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}