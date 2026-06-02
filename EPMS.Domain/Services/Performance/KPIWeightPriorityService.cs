using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Performance;

public class KPIWeightPriorityService : IKPIWeightPriorityService
{
    private readonly IUnitOfWork _uow;

    public KPIWeightPriorityService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<KPIWeightPriorityDto>>> GetAllAsync()
    {
        var priorities = await _uow.Perf.KPIWeightPriorities.GetAllAsync();
        var dtos = priorities.Adapt<IEnumerable<KPIWeightPriorityDto>>();
        return SuccessResponse<IEnumerable<KPIWeightPriorityDto>>.Ok(dtos, KPIWeightPriorityMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<KPIWeightPriorityDto>>> GetActiveAsync()
    {
        var priorities = await _uow.Perf.KPIWeightPriorities.GetActiveAsync();
        var dtos = priorities.Adapt<IEnumerable<KPIWeightPriorityDto>>();
        return SuccessResponse<IEnumerable<KPIWeightPriorityDto>>.Ok(dtos, KPIWeightPriorityMsg.RetrievedActive);
    }

    public async Task<SuccessResponse<KPIWeightPriorityDto>> GetByIdAsync(long id)
    {
        var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(id);

        if (priority == null)
            return SuccessResponse<KPIWeightPriorityDto>.Fail(KPIWeightPriorityMsg.NotFound(id), ErrorType.NotFound);

        var dto = priority.Adapt<KPIWeightPriorityDto>();
        return SuccessResponse<KPIWeightPriorityDto>.Ok(dto, KPIWeightPriorityMsg.Retrieved);
    }

    public async Task<SuccessResponse<KPIWeightPriorityDto>> GetByLevelNameAsync(string levelName)
    {
        var priority = await _uow.Perf.KPIWeightPriorities.GetByLevelNameAsync(levelName);

        if (priority == null)
            return SuccessResponse<KPIWeightPriorityDto>.Fail(KPIWeightPriorityMsg.NotFoundByLevelName(levelName), ErrorType.NotFound);

        var dto = priority.Adapt<KPIWeightPriorityDto>();
        return SuccessResponse<KPIWeightPriorityDto>.Ok(dto, KPIWeightPriorityMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateKPIWeightPriorityDto dto)
    {
        // Validate level name uniqueness
        if (await _uow.Perf.KPIWeightPriorities.LevelNameExistsAsync(dto.LevelName))
            return SuccessResponse<long>.Fail(string.Format(KPIWeightPriorityMsg.DuplicateLevelName, dto.LevelName), ErrorType.Conflict);

        // Validate weight bounds
        if (dto.MinWeight > dto.MaxWeight)
            return SuccessResponse<long>.Fail(KPIWeightPriorityMsg.MinGreaterThanMax, ErrorType.Validation);

        // Validate color code format if provided
        if (!string.IsNullOrEmpty(dto.ColorCode) && !IsValidHexColor(dto.ColorCode))
            return SuccessResponse<long>.Fail(KPIWeightPriorityMsg.InvalidColorCode, ErrorType.Validation);

        var priority = new KPIWeightPriority(dto.LevelName, dto.MinWeight, dto.MaxWeight, dto.ColorCode);

        _uow.Perf.KPIWeightPriorities.Add(priority);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(priority.Id, KPIWeightPriorityMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateKPIWeightPriorityDto dto)
    {
        var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(id);

        if (priority == null)
            return SuccessResponse.Fail(KPIWeightPriorityMsg.NotFound(id), ErrorType.NotFound);

        // Validate weight bounds if provided
        if (dto.MinWeight.HasValue && dto.MaxWeight.HasValue && dto.MinWeight.Value > dto.MaxWeight.Value)
            return SuccessResponse.Fail(KPIWeightPriorityMsg.MinGreaterThanMax, ErrorType.Validation);

        // Validate color code format if provided
        if (!string.IsNullOrEmpty(dto.ColorCode) && !IsValidHexColor(dto.ColorCode))
            return SuccessResponse.Fail(KPIWeightPriorityMsg.InvalidColorCode, ErrorType.Validation);

        // Update bounds if provided
        if (dto.MinWeight.HasValue || dto.MaxWeight.HasValue)
        {
            var minScore = dto.MinWeight ?? priority.MinWeight;
            var maxScore = dto.MaxWeight ?? priority.MaxWeight;
            priority.UpdateBounds(minScore, maxScore);
        }

        // Update color code if provided
        if (dto.ColorCode != null)
        {
            priority.UpdateDetails(priority.LevelName, dto.ColorCode);
        }

        if (dto.IsActive.HasValue)
        {
            if (dto.IsActive.Value) priority.Reactivate();
            else priority.Deactivate();
        }

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(KPIWeightPriorityMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(id);

        if (priority == null)
            return SuccessResponse.Fail(KPIWeightPriorityMsg.NotFound(id), ErrorType.NotFound);

        _uow.Perf.KPIWeightPriorities.Delete(priority);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(KPIWeightPriorityMsg.Deleted);
    }

    private static bool IsValidHexColor(string colorCode)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(colorCode, @"^#[0-9A-Fa-f]{6}$");
    }
    public async Task<SuccessResponse> RestoreAsync(long id)
    {
        var entity = await _uow.Perf.KPIWeightPriorities.GetByIdDeletedAsync(id);
        if (entity == null)
            return SuccessResponse.Fail(KPIWeightPriorityMsg.NotFound(id), ErrorType.NotFound);
        if (!entity.IsDeleted)
            return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = null;
        _uow.Perf.KPIWeightPriorities.Update(entity);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(KPIWeightPriorityMsg.Updated);
    }

}