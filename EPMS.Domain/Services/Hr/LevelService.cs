using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interfaces;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.LevelDTOs;
using EPMS.Shared.Enums;
using Mapster;

namespace EPMS.Domain.Services.Hr;

public class LevelService : ILevelService
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cacheService;

    public LevelService(IUnitOfWork uow, ICacheService cacheService)
    {
        _uow = uow;
        _cacheService = cacheService;
    }

    public async Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync()
    {
        var cachedAllLevels = await _cacheService.GetAsync<IEnumerable<LevelDto>>(CacheKeys.Hr.AllLevels());

        if (cachedAllLevels != null)
        {
            var lookupFromCache = cachedAllLevels.Select(x => new LookUpDto
            {
                Id = x.Id,
                Code = x.Code,
                IsActive = x.IsActive
            });

            return SuccessResponse<IEnumerable<LookUpDto>>.Ok(lookupFromCache, ServiceResponseMessages.LevelMsg.RetrievedAll);
        }

        var tuples = await _uow.HR.Levels.GetLookupAsync();

        var dtos = tuples.Select(t => new LookUpDto
        {
            Id = t.Id,
            Code = t.Code,
            IsActive = t.IsActive
        }).ToList();

        return SuccessResponse<IEnumerable<LookUpDto>>.Ok(dtos, ServiceResponseMessages.LevelMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<LevelDto>>> GetAllAsync()
    {
        var dtos = await _cacheService.GetOrCreateAsync(CacheKeys.Hr.AllLevels(), async () =>
        {
            var levels = await _uow.HR.Levels.GetAllAsync();
            return levels.Adapt<IEnumerable<LevelDto>>();
        });
        return SuccessResponse<IEnumerable<LevelDto>>.Ok(dtos, ServiceResponseMessages.LevelMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<LevelDto>> GetByIdAsync(long id)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse<LevelDto>.Fail(ServiceResponseMessages.LevelMsg.NotFound(id), ErrorType.NotFound);

        var dto = level.Adapt<LevelDto>();
        return SuccessResponse<LevelDto>.Ok(dto, ServiceResponseMessages.LevelMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateLevelDto dto)
    {
        if (await _uow.HR.Levels.ExistsByCodeAsync(dto.Code))
            return SuccessResponse<long>.Fail(string.Format(ServiceResponseMessages.LevelMsg.DuplicateCode, dto.Code.Trim().ToUpperInvariant()), ErrorType.Conflict);

        if (await _uow.HR.Levels.ExistsByNameAsync(dto.Name))
            return SuccessResponse<long>.Fail(string.Format(ServiceResponseMessages.LevelMsg.DuplicateName, dto.Name.Trim()), ErrorType.Conflict);

        var entity = new Level(dto.Code, dto.Name, dto.Description);
        _uow.HR.Levels.Add(entity);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllLevels());
        return SuccessResponse<long>.Ok(entity.Id, ServiceResponseMessages.LevelMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateLevelDto dto)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse.Fail(ServiceResponseMessages.LevelMsg.NotFound(id), ErrorType.NotFound);

        if (await _uow.HR.Levels.ExistsByNameAsync(dto.Name, id))
            return SuccessResponse.Fail(string.Format(ServiceResponseMessages.LevelMsg.DuplicateName, dto.Name.Trim()), ErrorType.Conflict);

        level.Update(dto.Name, dto.Description);

        if (dto.IsActive) level.Reactivate();
        else level.Deactivate();

        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllLevels());
        return SuccessResponse.Ok(ServiceResponseMessages.LevelMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse.Fail(ServiceResponseMessages.LevelMsg.NotFound(id), ErrorType.NotFound);

        if (await _uow.HR.Positions.AnyAsync(p => p.LevelId == id))
            return SuccessResponse.Fail(ServiceResponseMessages.LevelMsg.InUse(id), ErrorType.Conflict);

        _uow.HR.Levels.Delete(level);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllLevels());
        return SuccessResponse.Ok(ServiceResponseMessages.LevelMsg.Deleted);
    }
}
