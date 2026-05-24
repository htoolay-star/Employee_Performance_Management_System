using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

using Mapster;

namespace EPMS.Domain.Services.Performance
{
    public class FormTemplateService : IFormTemplateService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public FormTemplateService(IUnitOfWork uow, ICacheService cacheService)
        {
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<SuccessResponse<IEnumerable<FormTemplateDto>>> GetAllAsync()
        {
            var templates = await _uow.Perf.FormTemplates.GetAllWithQuestionsAsync();
            var dtos = templates.Select(t => new FormTemplateDto
            {
                Id = t.Id,
                Name = t.Name,
                FormType = t.FormType,
                RatingScaleId = t.QuestionRatingScaleId,
                RatingScaleName = t.RatingScale?.Name ?? string.Empty,
                QuestionsPerEvaluation = t.QuestionsPerEvaluation,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                QuestionCount = t.Questions?.Count ?? 0,
                HasYesNo = t.HasYesNo,
                HasComment = t.HasComment
            }).ToList();
            return SuccessResponse<IEnumerable<FormTemplateDto>>.Ok(dtos, FormTemplateMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<FormTemplateDto>>> GetActiveAsync()
        {
            var templates = await _uow.Perf.FormTemplates.GetActiveAsync();
            var dtos = templates.Adapt<IEnumerable<FormTemplateDto>>();
            return SuccessResponse<IEnumerable<FormTemplateDto>>.Ok(dtos, FormTemplateMsg.RetrievedActive);
        }

        public async Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync()
        {
            var lookups = await _cacheService.GetOrCreateAsync(
                CacheKeys.Performance.FormTemplateLookups(),
                async () =>
                {
                    var templates = await _uow.Perf.FormTemplates.FindAllAsync(
                        t => t.IsActive && !t.IsDeleted,
                        trackChanges: false);

                    return templates.Select(t => new LookUpDto { Id = t.Id, Name = t.Name, IsActive = t.IsActive }).ToList();
                },
                TimeSpan.FromHours(12)
            );

            return SuccessResponse<IEnumerable<LookUpDto>>.Ok(lookups ?? [], FormTemplateMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<FormTemplateDto>> GetByIdAsync(long id)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(id);

            if (template == null)
                return SuccessResponse<FormTemplateDto>.Fail(FormTemplateMsg.NotFound(id), ErrorType.NotFound);

            var dto = template.Adapt<FormTemplateDto>();
            dto.QuestionCount = template.Questions?.Count ?? 0;
            return SuccessResponse<FormTemplateDto>.Ok(dto, FormTemplateMsg.Retrieved);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateFormTemplateDto dto)
        {
            if (await _uow.Perf.FormTemplates.NameExistsAsync(dto.Name))
            {
                return SuccessResponse<long>.Fail(string.Format(FormTemplateMsg.DuplicateName, dto.Name), ErrorType.Conflict);
            }

            if (!await _uow.Perf.QuestionRatingScales.AnyAsync(s => s.Id == dto.RatingScaleId))
            {
                return SuccessResponse<long>.Fail("Rating scale not found.", ErrorType.NotFound);
            }

            var template = new FormTemplate(dto.Name, dto.FormType, dto.RatingScaleId, dto.QuestionsPerEvaluation, dto.HasYesNo, dto.HasComment);

            _uow.Perf.FormTemplates.Add(template);
            await _uow.CompleteAsync();

            await _cacheService.RemoveAsync(CacheKeys.Performance.FormTemplateLookups());

            return SuccessResponse<long>.Ok(template.Id, FormTemplateMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateFormTemplateDto dto)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(id);

            if (template == null)
                return SuccessResponse.Fail(FormTemplateMsg.NotFound(id), ErrorType.NotFound);

            if (await _uow.Perf.FormTemplates.NameExistsAsync(dto.Name, id))
            {
                return SuccessResponse.Fail(string.Format(FormTemplateMsg.DuplicateName, dto.Name), ErrorType.Conflict);
            }

            if (dto.RatingScaleId.HasValue && !await _uow.Perf.QuestionRatingScales.AnyAsync(s => s.Id == dto.RatingScaleId.Value))
            {
                return SuccessResponse.Fail("Rating scale not found.", ErrorType.NotFound);
            }

            if (dto.QuestionsPerEvaluation.HasValue)
            {
                var questionCount = template.Questions?.Count ?? 0;
                if (questionCount > 0 && dto.QuestionsPerEvaluation > questionCount)
                    return SuccessResponse.Fail(
                        $"Questions per evaluation ({dto.QuestionsPerEvaluation}) cannot exceed total questions ({questionCount}).",
                        ErrorType.Validation);
            }

            template.Update(dto.Name, dto.FormType, dto.RatingScaleId, dto.QuestionsPerEvaluation, dto.HasYesNo, dto.HasComment);

            if (dto.IsActive.HasValue)
            {
                if (dto.IsActive.Value) template.Reactivate();
                else template.Deactivate();
            }

            _uow.Perf.FormTemplates.Update(template);
            await _uow.CompleteAsync();
            await _cacheService.RemoveAsync(CacheKeys.Performance.FormTemplateLookups());

            return SuccessResponse.Ok(FormTemplateMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(id);

            if (template == null)
                return SuccessResponse.Fail(FormTemplateMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.FormTemplates.Delete(template);
            await _uow.CompleteAsync();

            await _cacheService.RemoveAsync(CacheKeys.Performance.FormTemplateLookups());

            return SuccessResponse.Ok(FormTemplateMsg.Deleted);
        }

        public async Task<SuccessResponse<FormTemplatePreviewDto>> GetPreviewAsync(long templateId)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(templateId);
            if (template == null)
                return SuccessResponse<FormTemplatePreviewDto>.Fail(FormTemplateMsg.NotFound(templateId), ErrorType.NotFound);

            var questions = await _uow.Perf.FormQuestions
                .FindAllAsync(q => q.TemplateId == templateId && !q.IsDeleted,
                    false, default, q => q.Category);

            var scaleWithLevels = await _uow.Perf.QuestionRatingScales
                .FindAllAsync(s => s.Id == template.QuestionRatingScaleId,
                    trackChanges: false,
                    includes: s => s.Levels);

            var scale = scaleWithLevels.FirstOrDefault();

            var preview = new FormTemplatePreviewDto
            {
                Id = template.Id,
                Name = template.Name,
                FormType = template.FormType,
                QuestionsPerEvaluation = template.QuestionsPerEvaluation,
                RatingScaleId = template.QuestionRatingScaleId,
                RatingScaleName = scale?.Name,
                RatingMaxScore = scale?.Levels.Any() == true ? (int)scale.Levels.Max(l => l.MaxScore) : null,
                HasYesNo = template.HasYesNo,
                HasComment = template.HasComment,
                Questions = questions.OrderBy(q => q.Sequence).Select(q => new FormTemplatePreviewQuestionDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    Sequence = q.Sequence,
                    CategoryId = q.CategoryId,
                    CategoryName = q.Category?.Name
                }).ToList()
            };

            return SuccessResponse<FormTemplatePreviewDto>.Ok(preview, FormTemplateMsg.Retrieved);
        }
    }
}
