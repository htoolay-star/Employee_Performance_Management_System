using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;
using EPMS.Domain.Interface.IService.Performance;

using Mapster;
namespace EPMS.Domain.Services.Performance;

public class EvaluationResponseService : IEvaluationResponseService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _timeProvider;
    
    public EvaluationResponseService(IUnitOfWork uow, TimeProvider timeProvider)
    {
        _uow = uow;
        _timeProvider = timeProvider;
    }

    public async Task<SuccessResponse> CreateAsync(CreateEvaluationResponseDto dto)
    {
        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(dto.AppraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(dto.AppraisalId), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        var template = await _uow.Perf.FormTemplates.GetByIdAsync(dto.TemplateId);
        if (template == null)
            return SuccessResponse.Fail(FormTemplateMsg.NotFound(dto.TemplateId), ErrorType.NotFound);

        var question = await _uow.Perf.FormQuestions.GetByIdAsync(dto.QuestionId);
        if (question == null)
            return SuccessResponse.Fail(FormQuestionMsg.NotFound(dto.QuestionId), ErrorType.NotFound);

        var evaluator = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EvaluatorId);
        if (evaluator == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(dto.EvaluatorId), ErrorType.NotFound);

        var response = new EvaluationResponse(
            dto.AppraisalId,
            dto.TemplateId,
            dto.QuestionId,
            dto.EvaluatorId,
            dto.EvaluatorRole,
            dto.IsAnonymous);

        if (dto.YesNoAnswer.HasValue)
            response.SetYesNo(dto.YesNoAnswer.Value);

        if (!string.IsNullOrWhiteSpace(dto.Comment))
            response.AddComment(dto.Comment);

        _uow.Perf.EvaluationResponses.Add(response);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(EvaluationResponseMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEvaluationResponseDto dto)
    {
        var response = await _uow.Perf.EvaluationResponses.GetByIdWithDetailsAsync(id);
        if (response == null)
            return SuccessResponse.Fail(EvaluationResponseMsg.NotFound(id), ErrorType.NotFound);

        if (response.Appraisal != null && response.Appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        response.UpdateDetails(dto.YesNoAnswer, dto.RatingValue, dto.Comment);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(EvaluationResponseMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var response = await _uow.Perf.EvaluationResponses.GetByIdAsync(id);
        if (response == null)
            return SuccessResponse.Fail(EvaluationResponseMsg.NotFound(id), ErrorType.NotFound);

        if (response.Appraisal != null && response.Appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        response.IsDeleted = true;
        response.DeletedAt = _timeProvider.GetUtcNow();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(EvaluationResponseMsg.Deleted);
    }

    public async Task<SuccessResponse> GetByIdAsync(long id)
    {
        var response = await _uow.Perf.EvaluationResponses.GetByIdWithDetailsAsync(id);
        if (response == null)
            return SuccessResponse.Fail(EvaluationResponseMsg.NotFound(id), ErrorType.NotFound);

        var dto = response.Adapt<EvaluationResponseDto>();
        return SuccessResponse<EvaluationResponseDto>.Ok(dto, EvaluationResponseMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAllAsync()
    {
        var responses = await _uow.Perf.EvaluationResponses.GetAllAsync();
        var dtos = responses.Where(r => !r.IsDeleted).Adapt<IEnumerable<EvaluationResponseDto>>();
        return SuccessResponse<IEnumerable<EvaluationResponseDto>>.Ok(dtos, EvaluationResponseMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByAppraisalIdAsync(long appraisalId)
    {
        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(appraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        var responses = await _uow.Perf.EvaluationResponses.GetByAppraisalIdAsync(appraisalId);
        var dtos = responses.Where(r => !r.IsDeleted).Adapt<IEnumerable<EvaluationResponseDto>>();
        return SuccessResponse<IEnumerable<EvaluationResponseDto>>.Ok(dtos, EvaluationResponseMsg.RetrievedByAppraisal);
    }

    public async Task<SuccessResponse> GetByTemplateIdAsync(long templateId)
    {
        var responses = await _uow.Perf.EvaluationResponses.GetByTemplateIdAsync(templateId);
        var dtos = responses.Where(r => !r.IsDeleted).Adapt<IEnumerable<EvaluationResponseDto>>();
        return SuccessResponse<IEnumerable<EvaluationResponseDto>>.Ok(dtos, EvaluationResponseMsg.RetrievedByTemplate);
    }

    public async Task<SuccessResponse> GetByQuestionIdAsync(long questionId)
    {
        var responses = await _uow.Perf.EvaluationResponses.GetByQuestionIdAsync(questionId);
        var dtos = responses.Where(r => !r.IsDeleted).Adapt<IEnumerable<EvaluationResponseDto>>();
        return SuccessResponse<IEnumerable<EvaluationResponseDto>>.Ok(dtos, EvaluationResponseMsg.RetrievedByQuestion);
    }
}
