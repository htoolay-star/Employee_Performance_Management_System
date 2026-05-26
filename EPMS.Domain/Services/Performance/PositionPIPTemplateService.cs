using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Performance;

public class PositionPIPTemplateService : IPositionPIPTemplateService
{
    private readonly IUnitOfWork _uow;

    public PositionPIPTemplateService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetAllAsync()
    {
        var templates = await _uow.Perf.PositionPIPTemplates.GetAllAsync();
        var dtos = templates.Adapt<IEnumerable<PositionPIPTemplateDto>>();
        return SuccessResponse<IEnumerable<PositionPIPTemplateDto>>.Ok(dtos, PositionPIPTemplateMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<PositionPIPTemplateDto>> GetByIdAsync(long id)
    {
        var template = await _uow.Perf.PositionPIPTemplates.GetByIdAsync(id);

        if (template == null)
            return SuccessResponse<PositionPIPTemplateDto>.Fail(PositionPIPTemplateMsg.NotFound(id), ErrorType.NotFound);

        var dto = template.Adapt<PositionPIPTemplateDto>();
        return SuccessResponse<PositionPIPTemplateDto>.Ok(dto, PositionPIPTemplateMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetByPositionIdAsync(long positionId)
    {
        var templates = await _uow.Perf.PositionPIPTemplates.GetByPositionIdAsync(positionId);
        var dtos = templates.Adapt<IEnumerable<PositionPIPTemplateDto>>();
        return SuccessResponse<IEnumerable<PositionPIPTemplateDto>>.Ok(dtos, PositionPIPTemplateMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetActiveByPositionIdAsync(long positionId)
    {
        var templates = await _uow.Perf.PositionPIPTemplates.GetActiveByPositionIdAsync(positionId);
        var dtos = templates.Adapt<IEnumerable<PositionPIPTemplateDto>>();
        return SuccessResponse<IEnumerable<PositionPIPTemplateDto>>.Ok(dtos, PositionPIPTemplateMsg.RetrievedActive);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreatePositionPIPTemplateDto dto)
    {
        if (!await _uow.HR.Positions.ExistsByIdAsync(dto.PositionId))
            return SuccessResponse<long>.Fail(PositionMsg.NotFound(dto.PositionId), ErrorType.NotFound);

        var template = new PositionPIPTemplate(dto.PositionId, dto.Title, dto.SuccessCriteria, dto.Description);

        _uow.Perf.PositionPIPTemplates.Add(template);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(template.Id, PositionPIPTemplateMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdatePositionPIPTemplateDto dto)
    {
        var template = await _uow.Perf.PositionPIPTemplates.GetByIdAsync(id);

        if (template == null)
            return SuccessResponse.Fail(PositionPIPTemplateMsg.NotFound(id), ErrorType.NotFound);

        template.UpdateDetails(
            dto.Title ?? template.Title,
            dto.SuccessCriteria ?? template.SuccessCriteria,
            dto.Description);

        if (dto.IsActive.HasValue)
        {
            if (dto.IsActive.Value) template.Reactivate();
            else template.Deactivate();
        }

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(PositionPIPTemplateMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var template = await _uow.Perf.PositionPIPTemplates.GetByIdAsync(id);

        if (template == null)
            return SuccessResponse.Fail(PositionPIPTemplateMsg.NotFound(id), ErrorType.NotFound);

        _uow.Perf.PositionPIPTemplates.Delete(template);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(PositionPIPTemplateMsg.Deleted);
    }
}
