using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Performance;

public class QuestionRatingScaleService : IQuestionRatingScaleService
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cacheService;

    public QuestionRatingScaleService(IUnitOfWork uow, ICacheService cacheService)
    {
        _uow = uow;
        _cacheService = cacheService;
    }

    public async Task<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>> GetAllAsync()
    {
        var scales = await _uow.Perf.QuestionRatingScales.FindAllAsync(
            s => !s.IsDeleted,
            trackChanges: false,
            includes: s => s.Levels);

        var dtos = scales.Adapt<IEnumerable<QuestionRatingScaleDto>>();
        return SuccessResponse<IEnumerable<QuestionRatingScaleDto>>.Ok(dtos, QuestionRatingScaleMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<QuestionRatingScaleDto>> GetByIdAsync(long id)
    {
        var scale = await _uow.Perf.QuestionRatingScales.FindAsync(
            s => s.Id == id,
            trackChanges: false,
            includes: s => s.Levels);

        if (scale == null)
            return SuccessResponse<QuestionRatingScaleDto>.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);

        var dto = scale.Adapt<QuestionRatingScaleDto>();
        return SuccessResponse<QuestionRatingScaleDto>.Ok(dto, QuestionRatingScaleMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>> GetActiveAsync()
    {
        var scales = await _uow.Perf.QuestionRatingScales.FindAllAsync(
            s => s.IsActive && !s.IsDeleted,
            trackChanges: false,
            includes: s => s.Levels);

        var dtos = scales.Adapt<IEnumerable<QuestionRatingScaleDto>>();
        return SuccessResponse<IEnumerable<QuestionRatingScaleDto>>.Ok(dtos, QuestionRatingScaleMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync()
    {
        var lookups = await _cacheService.GetOrCreateAsync(
            CacheKeys.Performance.QuestionRatingScaleLookups(),
            async () =>
            {
                var scales = await _uow.Perf.QuestionRatingScales.FindAllAsync(
                    s => s.IsActive && !s.IsDeleted,
                    trackChanges: false);

                return scales.Select(s => new LookUpDto { Id = s.Id, Name = s.Name, IsActive = s.IsActive }).ToList();
            },
            TimeSpan.FromHours(12)
        );

        return SuccessResponse<IEnumerable<LookUpDto>>.Ok(lookups ?? [], QuestionRatingScaleMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateQuestionRatingScaleDto dto)
    {
        var scale = new QuestionRatingScale(dto.Name);

        var levels = dto.Levels.Select(l =>
            new QuestionRatingScaleLevel(0, l.Rating, l.MinScore, l.MaxScore)).ToList();

        scale.SetLevels(levels);

        _uow.Perf.QuestionRatingScales.Add(scale);
        await _uow.CompleteAsync();

        foreach (var level in levels)
        {
            level.GetType().GetProperty("QuestionRatingScaleId")!.SetValue(level, scale.Id);
        }
        await _uow.CompleteAsync();

        await _cacheService.RemoveAsync(CacheKeys.Performance.QuestionRatingScaleLookups());

        return SuccessResponse<long>.Ok(scale.Id, QuestionRatingScaleMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateQuestionRatingScaleDto dto)
    {
        var scale = await _uow.Perf.QuestionRatingScales.FindAsync(
            s => s.Id == id,
            trackChanges: false,
            includes: s => s.Levels);

        if (scale == null)
            return SuccessResponse.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);

        if (dto.Name != null)
            scale.Rename(dto.Name);

        if (dto.IsActive.HasValue)
        {
            if (dto.IsActive.Value)
                scale.Reactivate();
            else
                scale.Deactivate();
        }

        if (dto.Levels != null && dto.Levels.Any())
        {
            var existingLevels = scale.Levels.ToList();
            var incomingIds = dto.Levels.Where(l => l.Id.HasValue).Select(l => l.Id!.Value).ToHashSet();

            foreach (var existing in existingLevels)
            {
                if (!incomingIds.Contains(existing.Id))
                {
                    _uow.Perf.QuestionRatingScaleLevels.Delete(existing);
                }
            }

            foreach (var levelDto in dto.Levels)
            {
                if (levelDto.Id.HasValue)
                {
                    var existing = existingLevels.FirstOrDefault(l => l.Id == levelDto.Id);
                    if (existing != null)
                    {
                        existing.UpdateBounds(levelDto.MinScore, levelDto.MaxScore);
                        _uow.Perf.QuestionRatingScaleLevels.Update(existing);
                    }
                }
                else
                {
                    var newLevel = new QuestionRatingScaleLevel(scale.Id, levelDto.Rating, levelDto.MinScore, levelDto.MaxScore);
                    _uow.Perf.QuestionRatingScaleLevels.Add(newLevel);
                }
            }
        }

        _uow.Perf.QuestionRatingScales.Update(scale);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Performance.QuestionRatingScaleLookups());
        return SuccessResponse.Ok(QuestionRatingScaleMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var scale = await _uow.Perf.QuestionRatingScales.FindAsync(
            s => s.Id == id,
            trackChanges: true,
            includes: s => s.Levels);

        if (scale == null)
            return SuccessResponse.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);

        _uow.Perf.QuestionRatingScales.Delete(scale);
        await _uow.CompleteAsync();

        await _cacheService.RemoveAsync(CacheKeys.Performance.QuestionRatingScaleLookups());

        return SuccessResponse.Ok(QuestionRatingScaleMsg.Deleted);
    }

    public async Task<SuccessResponse> RestoreAsync(long id)
    {
        var entity = await _uow.Perf.QuestionRatingScales.GetByIdDeletedAsync(id);

        if (entity == null)
            return SuccessResponse.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);
        if (!entity.IsDeleted)
            return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);

        var deletedLevels = await _uow.Perf.QuestionRatingScaleLevels.GetAllDeletedAsync();
        var childLevels = deletedLevels.Cast<QuestionRatingScaleLevel>()
            .Where(l => l.QuestionRatingScaleId == id)
            .ToList();

        foreach (var level in childLevels)
        {
            level.IsDeleted = false;
            level.DeletedAt = null;
            level.DeletedBy = null;
            _uow.Perf.QuestionRatingScaleLevels.Update(level);
        }

        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = null;
        _uow.Perf.QuestionRatingScales.Update(entity);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(QuestionRatingScaleMsg.Updated);
    }
}
