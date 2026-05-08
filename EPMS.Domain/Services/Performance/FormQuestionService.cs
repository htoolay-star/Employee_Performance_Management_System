using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Performance;

public class FormQuestionService : IFormQuestionService
{
    private readonly IUnitOfWork _uow;

    public FormQuestionService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<SuccessResponse> CreateAsync(CreateFormQuestionDto dto)
    {
        var template = await _uow.Perf.FormTemplates.GetByIdAsync(dto.TemplateId);
        if (template == null)
            return SuccessResponse.Fail(FormTemplateMsg.NotFound(dto.TemplateId), ErrorType.NotFound);

        var exists = await _uow.Perf.FormQuestions.ExistsAsync(dto.TemplateId, dto.Sequence);
        if (exists)
            return SuccessResponse.Fail(FormQuestionMsg.DuplicateEntry, ErrorType.Conflict);

        var formQuestion = new FormQuestion(
            dto.TemplateId,
            dto.QuestionText,
            dto.Sequence,
            dto.HasYesNo,
            dto.HasComment,
            dto.CategoryId,
            dto.RatingScaleId);

        _uow.Perf.FormQuestions.Add(formQuestion);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(FormQuestionMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateFormQuestionDto dto)
    {
        var formQuestion = await _uow.Perf.FormQuestions.GetByIdAsync(id);
        if (formQuestion == null)
            return SuccessResponse.Fail(FormQuestionMsg.NotFound(id), ErrorType.NotFound);

        if (dto.QuestionText != null)
        {
            formQuestion.UpdateDetails(dto.QuestionText, dto.CategoryId, dto.RatingScaleId);
        }
        else if (dto.CategoryId.HasValue || dto.RatingScaleId.HasValue)
        {
            formQuestion.UpdateDetails(formQuestion.QuestionText, dto.CategoryId, dto.RatingScaleId);
        }

        if (dto.Sequence.HasValue && dto.Sequence.Value != formQuestion.Sequence)
        {
            var newSequenceExists = await _uow.Perf.FormQuestions.ExistsAsync(formQuestion.TemplateId, dto.Sequence.Value);
            if (newSequenceExists && dto.Sequence.Value != formQuestion.Sequence)
                return SuccessResponse.Fail(FormQuestionMsg.DuplicateEntry, ErrorType.Conflict);

            formQuestion.ChangeSequence(dto.Sequence.Value);
        }

        if (dto.HasYesNo.HasValue || dto.HasComment.HasValue)
        {
            formQuestion.ToggleUIControls(
                dto.HasYesNo ?? formQuestion.HasYesNo,
                dto.HasComment ?? formQuestion.HasComment);
        }

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(FormQuestionMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var formQuestion = await _uow.Perf.FormQuestions.GetByIdAsync(id);
        if (formQuestion == null)
            return SuccessResponse.Fail(FormQuestionMsg.NotFound(id), ErrorType.NotFound);

        formQuestion.IsDeleted = true;
        formQuestion.DeletedAt = DateTimeOffset.UtcNow;

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(FormQuestionMsg.Deleted);
    }

    public async Task<SuccessResponse> GetByIdAsync(long id)
    {
        var formQuestion = await _uow.Perf.FormQuestions.GetByIdAsync(id);
        if (formQuestion == null)
            return SuccessResponse.Fail(FormQuestionMsg.NotFound(id), ErrorType.NotFound);

        var dto = MapToDto(formQuestion);
        return SuccessResponse<FormQuestionDto>.Ok(dto, FormQuestionMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAllAsync()
    {
        var formQuestions = await _uow.Perf.FormQuestions.GetAllAsync();
        var dtos = formQuestions.Where(q => !q.IsDeleted).Select(MapToDto);
        return SuccessResponse<IEnumerable<FormQuestionDto>>.Ok(dtos, FormQuestionMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByTemplateIdAsync(long templateId)
    {
        var template = await _uow.Perf.FormTemplates.GetByIdAsync(templateId);
        if (template == null)
            return SuccessResponse.Fail(FormTemplateMsg.NotFound(templateId), ErrorType.NotFound);

        var formQuestions = await _uow.Perf.FormQuestions.GetByTemplateIdAsync(templateId);
        var dtos = formQuestions.Select(MapToDto);
        return SuccessResponse<IEnumerable<FormQuestionDto>>.Ok(dtos, FormQuestionMsg.RetrievedByTemplate);
    }

    public async Task<SuccessResponse> GetByCategoryIdAsync(long categoryId)
    {
        var formQuestions = await _uow.Perf.FormQuestions.GetByCategoryIdAsync(categoryId);
        var dtos = formQuestions.Select(MapToDto);
        return SuccessResponse<IEnumerable<FormQuestionDto>>.Ok(dtos, FormQuestionMsg.RetrievedByCategory);
    }

    private static FormQuestionDto MapToDto(FormQuestion fq)
    {
        return new FormQuestionDto(
            fq.Id,
            fq.TemplateId,
            fq.QuestionText,
            fq.Sequence,
            fq.HasYesNo,
            fq.HasComment,
            fq.CategoryId,
            fq.Category?.Name,
            fq.QuestionRatingScaleId,
            fq.RatingScale?.Name,
            fq.Tags.Select(t => t.Name).ToList());
    }
}