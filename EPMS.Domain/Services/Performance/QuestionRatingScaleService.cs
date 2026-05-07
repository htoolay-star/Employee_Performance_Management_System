using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Performance;

public class QuestionRatingScaleService : IQuestionRatingScaleService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public QuestionRatingScaleService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>> GetAllAsync()
    {
        var scales = await _uow.Perf.QuestionRatingScales.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<QuestionRatingScaleDto>>(scales);
        return SuccessResponse<IEnumerable<QuestionRatingScaleDto>>.Ok(dtos, QuestionRatingScaleMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<QuestionRatingScaleDto>> GetByIdAsync(long id)
    {
        var scale = await _uow.Perf.QuestionRatingScales.GetByIdAsync(id);

        if (scale == null)
            return SuccessResponse<QuestionRatingScaleDto>.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<QuestionRatingScaleDto>(scale);
        return SuccessResponse<QuestionRatingScaleDto>.Ok(dto, QuestionRatingScaleMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>> GetActiveAsync()
    {
        var scales = await _uow.Perf.QuestionRatingScales.GetAllAsync();
        var activeScales = scales.Where(s => s.IsActive).ToList();
        var dtos = _mapper.Map<IEnumerable<QuestionRatingScaleDto>>(activeScales);
        return SuccessResponse<IEnumerable<QuestionRatingScaleDto>>.Ok(dtos, QuestionRatingScaleMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateQuestionRatingScaleDto dto)
    {
        var scale = new QuestionRatingScale(dto.Name, dto.MinScore, dto.MaxScore);

        _uow.Perf.QuestionRatingScales.Add(scale);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(scale.Id, QuestionRatingScaleMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateQuestionRatingScaleDto dto)
    {
        var scale = await _uow.Perf.QuestionRatingScales.GetByIdAsync(id);

        if (scale == null)
            return SuccessResponse.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);

        if (dto.Name != null)
            scale.Rename(dto.Name);

        if (dto.MinScore.HasValue || dto.MaxScore.HasValue)
        {
            var minScore = dto.MinScore ?? scale.MinScore;
            var maxScore = dto.MaxScore ?? scale.MaxScore;
            scale.UpdateBounds(minScore, maxScore);
        }

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(QuestionRatingScaleMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var scale = await _uow.Perf.QuestionRatingScales.GetByIdAsync(id);

        if (scale == null)
            return SuccessResponse.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);

        _uow.Perf.QuestionRatingScales.Delete(scale);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(QuestionRatingScaleMsg.Deleted);
    }

    public async Task<SuccessResponse> DeactivateAsync(long id)
    {
        var scale = await _uow.Perf.QuestionRatingScales.GetByIdAsync(id);

        if (scale == null)
            return SuccessResponse.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);

        scale.Deactivate();
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(QuestionRatingScaleMsg.Updated);
    }

    public async Task<SuccessResponse> ReactivateAsync(long id)
    {
        var scale = await _uow.Perf.QuestionRatingScales.GetByIdAsync(id);

        if (scale == null)
            return SuccessResponse.Fail(QuestionRatingScaleMsg.NotFound(id), ErrorType.NotFound);

        scale.Reactivate();
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(QuestionRatingScaleMsg.Updated);
    }
}