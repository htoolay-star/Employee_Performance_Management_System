using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PositionFormTemplateDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Performance;

public class PositionFormTemplateService : IPositionFormTemplateService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PositionFormTemplateService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<PositionFormTemplateDto>>> GetAllAsync()
    {
        var templates = await _uow.Perf.PositionFormTemplates.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<PositionFormTemplateDto>>(templates);
        return SuccessResponse<IEnumerable<PositionFormTemplateDto>>.Ok(dtos, PositionFormTemplateMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<PositionFormTemplateDto>> GetByIdAsync(long id)
    {
        var template = await _uow.Perf.PositionFormTemplates.GetByIdAsync(id);

        if (template == null)
            return SuccessResponse<PositionFormTemplateDto>.Fail(PositionFormTemplateMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<PositionFormTemplateDto>(template);
        return SuccessResponse<PositionFormTemplateDto>.Ok(dto, PositionFormTemplateMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<PositionFormTemplateDto>>> GetByPositionIdAsync(long positionId)
    {
        var templates = await _uow.Perf.PositionFormTemplates.GetByPositionIdAsync(positionId);
        var dtos = _mapper.Map<IEnumerable<PositionFormTemplateDto>>(templates);
        return SuccessResponse<IEnumerable<PositionFormTemplateDto>>.Ok(dtos, PositionFormTemplateMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreatePositionFormTemplateDto dto)
    {
        if (!await _uow.HR.Positions.ExistsByIdAsync(dto.PositionId))
            return SuccessResponse<long>.Fail(PositionMsg.NotFound(dto.PositionId), ErrorType.NotFound);

        var formTemplate = await _uow.Perf.FormTemplates.GetByIdAsync(dto.FormTemplateId);
        if (formTemplate == null)
            return SuccessResponse<long>.Fail(FormTemplateMsg.NotFound(dto.FormTemplateId), ErrorType.NotFound);

        if (await _uow.Perf.PositionFormTemplates.ExistsAsync(dto.PositionId, dto.FormTemplateId))
            return SuccessResponse<long>.Fail(PositionFormTemplateMsg.DuplicateEntry, ErrorType.Conflict);

        var template = new PositionFormTemplate(dto.PositionId, dto.FormTemplateId, dto.IsMandatory);

        _uow.Perf.PositionFormTemplates.Add(template);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(template.Id, PositionFormTemplateMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdatePositionFormTemplateDto dto)
    {
        var template = await _uow.Perf.PositionFormTemplates.GetByIdAsync(id);

        if (template == null)
            return SuccessResponse.Fail(PositionFormTemplateMsg.NotFound(id), ErrorType.NotFound);

        template.ToggleMandatory(dto.IsMandatory);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(PositionFormTemplateMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var template = await _uow.Perf.PositionFormTemplates.GetByIdAsync(id);

        if (template == null)
            return SuccessResponse.Fail(PositionFormTemplateMsg.NotFound(id), ErrorType.NotFound);

        _uow.Perf.PositionFormTemplates.Delete(template);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(PositionFormTemplateMsg.Deleted);
    }
}