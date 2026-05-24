using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;
using EPMS.Domain.Interface.IService.Performance;
using System.Linq.Expressions;

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

        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(appraisal.CycleId);
        if (cycle == null)
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(appraisal.CycleId), ErrorType.NotFound);

        var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
        var (start, deadline) = dto.EvaluatorRole switch
        {
            EvaluatorRoles.Self => (cycle.SelfReviewStartDate, cycle.SelfReviewDeadline),
            EvaluatorRoles.Manager or EvaluatorRoles.Peer or EvaluatorRoles.Subordinate => (cycle.ThreeSixtyReviewStartDate, cycle.ThreeSixtyReviewDeadline),
            EvaluatorRoles.Appraisal => (cycle.ManagerReviewStartDate as DateOnly?, cycle.ManagerReviewDeadline as DateOnly?),
            _ => (cycle.WindowStartDate, cycle.WindowEndDate)
        };
        start ??= cycle.WindowStartDate;
        deadline ??= cycle.WindowEndDate;

        if (today < start)
            return SuccessResponse.Fail($"{dto.EvaluatorRole} review window opens on {start:dd/MM/yyyy}.", ErrorType.Validation);
        if (today > deadline)
            return SuccessResponse.Fail($"{dto.EvaluatorRole} review window closed on {deadline:dd/MM/yyyy}.", ErrorType.Validation);

        if (dto.EvaluatorRole == EvaluatorRoles.Self && appraisal.SelfLocked)
            return SuccessResponse.Fail("Self evaluation is locked.", ErrorType.Conflict);
        if ((dto.EvaluatorRole == EvaluatorRoles.Manager || dto.EvaluatorRole == EvaluatorRoles.Peer || dto.EvaluatorRole == EvaluatorRoles.Subordinate) && appraisal.ThreeSixtyLocked)
            return SuccessResponse.Fail("360 evaluation is locked.", ErrorType.Conflict);
        if (dto.EvaluatorRole == EvaluatorRoles.Appraisal && appraisal.AppraisalLocked)
            return SuccessResponse.Fail("Appraisal evaluation is locked.", ErrorType.Conflict);

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

        if (response.SubmittedAt.HasValue)
            return SuccessResponse.Fail("This response has already been submitted.", ErrorType.Conflict);

        if (response.Appraisal != null)
        {
            var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(response.Appraisal.CycleId);
            if (cycle != null)
            {
                var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
                var (start, deadline) = response.EvaluatorRole switch
                {
                    EvaluatorRoles.Self => (cycle.SelfReviewStartDate, cycle.SelfReviewDeadline),
                    EvaluatorRoles.Manager or EvaluatorRoles.Peer or EvaluatorRoles.Subordinate => (cycle.ThreeSixtyReviewStartDate, cycle.ThreeSixtyReviewDeadline),
                    EvaluatorRoles.Appraisal => (cycle.ManagerReviewStartDate as DateOnly?, cycle.ManagerReviewDeadline as DateOnly?),
                    _ => (cycle.WindowStartDate, cycle.WindowEndDate)
                };
                start ??= cycle.WindowStartDate;
                deadline ??= cycle.WindowEndDate;

                if (today < start)
                    return SuccessResponse.Fail($"{response.EvaluatorRole} review window opens on {start:dd/MM/yyyy}.", ErrorType.Validation);
                if (today > deadline)
                    return SuccessResponse.Fail($"{response.EvaluatorRole} review window closed on {deadline:dd/MM/yyyy}.", ErrorType.Validation);
            }
        }

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

    public async Task<SuccessResponse> GetFormFillAsync(long appraisalId, long evaluatorId, string role)
    {
        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(appraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        var responses = await _uow.Perf.EvaluationResponses
            .FindAllAsync(r => r.AppraisalId == appraisalId
                            && r.EvaluatorId == evaluatorId
                            && r.EvaluatorRole == role
                            && !r.IsDeleted,
                          trackChanges: false,
                          includes: new Expression<Func<EvaluationResponse, object>>[] { r => r.Question, r => r.Question.Category, r => r.Template });

        var templateIds = responses
            .Select(r => r.TemplateId)
            .Distinct()
            .ToList();

        var templateScales = new Dictionary<long, long>(); // templateId -> questionRatingScaleId
        if (templateIds.Count != 0)
        {
            var templates = await _uow.Perf.FormTemplates
                .FindAllAsync(t => templateIds.Contains(t.Id), trackChanges: false);

            foreach (var t in templates)
            {
                templateScales[t.Id] = t.QuestionRatingScaleId;
            }
        }

        var scaleIds = templateScales.Values.Distinct().ToList();
        var scalesWithLevels = new Dictionary<long, List<RatingLevelDto>>();
        if (scaleIds.Count != 0)
        {
            var loaded = await _uow.Perf.QuestionRatingScales
                .FindAllAsync(s => scaleIds.Contains(s.Id),
                    trackChanges: false,
                    includes: s => s.Levels);

            foreach (var s in loaded)
            {
                scalesWithLevels[s.Id] = s.Levels
                    .OrderBy(l => l.Rating)
                    .Select(l => new RatingLevelDto { Rating = l.Rating, MinScore = l.MinScore, MaxScore = l.MaxScore })
                    .ToList();
            }
        }

        var questions = responses.Select(r =>
        {
            var scaleId = r.TemplateId != 0 && templateScales.TryGetValue(r.TemplateId, out var sid) ? sid : 0;
            return new EvaluationFormQuestionItem
            {
                ResponseId = r.Id,
                TemplateId = r.TemplateId,
                TemplateName = r.Template?.Name,
                QuestionId = r.QuestionId,
                QuestionText = r.Question?.QuestionText,
                CategoryName = r.Question?.Category?.Name,
                Sequence = r.Question?.Sequence ?? 0,
                HasYesNo = r.Template?.HasYesNo ?? false,
                HasComment = r.Template?.HasComment ?? false,
                RatingLevels = scalesWithLevels.TryGetValue(scaleId, out var levels) ? levels : null,
                MaxScore = scalesWithLevels.TryGetValue(scaleId, out var lvl) ? (int?)lvl.Max(l => l.Rating) : null,
                YesNoAnswer = r.YesNoAnswer,
                RatingValue = r.RatingValue,
                Comment = r.QuestionComment
            };
        }).OrderBy(q => q.TemplateId).ThenBy(q => q.Sequence).ToList();

        var submitted = responses.All(r => r.SubmittedAt.HasValue);

        decimal? totalPoint = null;
        string? ratingLabel = null;
        var answeredResponses = responses.Where(r => r.RatingValue.HasValue).ToList();
        if (answeredResponses.Any())
        {
            var sum = answeredResponses.Sum(r => r.RatingValue!.Value);
            var count = answeredResponses.Count;
            var maxRating = questions.FirstOrDefault(q => q.MaxScore.HasValue)?.MaxScore ?? 5;
            totalPoint = (decimal)sum * 100m / (count * maxRating);

            var ratingScales = await _uow.Perf.RatingScales
                .FindAllAsync(s => s.IsActive && !s.IsDeleted);
            var matchingScale = ratingScales.FirstOrDefault(s => s.IsMatch(totalPoint.Value));
            ratingLabel = matchingScale?.Label;
        }

        var dto = new EvaluationFormFillDto(
            appraisal.Id,
            appraisal.Employee?.StaffName,
            appraisal.Employee?.StaffNo,
            appraisal.Employee?.Employment?.Position?.Name,
            appraisal.Employee?.Employment?.Department?.Name,
            appraisal.CycleId,
            appraisal.Cycle?.Name,
            appraisal.Status,
            role,
            submitted,
            appraisal.IsLocked,
            questions,
            totalPoint,
            ratingLabel
        );

        return SuccessResponse<EvaluationFormFillDto>.Ok(dto, EvaluationResponseMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetSelfAssessmentAsync(long appraisalId, long managerId)
    {
        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(appraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        if (managerId != appraisal.ManagerReviewerId)
            return SuccessResponse.Fail("Only the manager reviewer can view the self-assessment.", ErrorType.Forbidden);

        return await GetFormFillAsync(appraisalId, appraisal.EmployeeId ?? 0, EvaluatorRoles.Self);
    }

    public async Task<SuccessResponse> SubmitRoleResponsesAsync(long appraisalId, long evaluatorId, string role)
    {
        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(appraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        if (role == EvaluatorRoles.Self && appraisal.SelfLocked)
            return SuccessResponse.Fail("Self evaluation is locked.", ErrorType.Conflict);
        if ((role == EvaluatorRoles.Manager || role == EvaluatorRoles.Peer || role == EvaluatorRoles.Subordinate) && appraisal.ThreeSixtyLocked)
            return SuccessResponse.Fail("360 evaluation is locked.", ErrorType.Conflict);
        if (role == EvaluatorRoles.Appraisal && appraisal.AppraisalLocked)
            return SuccessResponse.Fail("Appraisal evaluation is locked.", ErrorType.Conflict);

        if (appraisal.Cycle != null)
        {
            var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
            var (start, deadline) = role switch
            {
                EvaluatorRoles.Self => (appraisal.Cycle.SelfReviewStartDate, appraisal.Cycle.SelfReviewDeadline),
                EvaluatorRoles.Manager or EvaluatorRoles.Peer or EvaluatorRoles.Subordinate => (appraisal.Cycle.ThreeSixtyReviewStartDate, appraisal.Cycle.ThreeSixtyReviewDeadline),
                EvaluatorRoles.Appraisal => (appraisal.Cycle.ManagerReviewStartDate as DateOnly?, appraisal.Cycle.ManagerReviewDeadline as DateOnly?),
                _ => (appraisal.Cycle.WindowStartDate, appraisal.Cycle.WindowEndDate)
            };
            start ??= appraisal.Cycle.WindowStartDate;
            deadline ??= appraisal.Cycle.WindowEndDate;

            if (today < start)
                return SuccessResponse.Fail($"{role} review window opens on {start:dd/MM/yyyy}.", ErrorType.Validation);
            if (today > deadline)
                return SuccessResponse.Fail($"{role} review window closed on {deadline:dd/MM/yyyy}.", ErrorType.Validation);
        }

        var responses = await _uow.Perf.EvaluationResponses
            .FindAllAsync(r => r.AppraisalId == appraisalId && r.EvaluatorId == evaluatorId
                            && r.EvaluatorRole == role && !r.IsDeleted,
                          trackChanges: true,
                          includes: r => r.Question);

        if (!responses.Any())
            return SuccessResponse.Fail("No responses found to submit.", ErrorType.NotFound);

        if (responses.All(r => r.SubmittedAt.HasValue))
            return SuccessResponse.Fail("All responses have already been submitted.", ErrorType.Conflict);

        var missingRating = responses.Where(r => !r.RatingValue.HasValue).ToList();
        if (missingRating.Any())
            return SuccessResponse.Fail("Please provide a rating for all questions before submitting.", ErrorType.Validation);

        var missingYesNo = responses.Where(r => r.Template?.HasYesNo == true && !r.YesNoAnswer.HasValue).ToList();
        if (missingYesNo.Any())
            return SuccessResponse.Fail("Please answer Yes/No for all applicable questions before submitting.", ErrorType.Validation);

        var now = _timeProvider.GetUtcNow();
        foreach (var response in responses)
            response.Submit(_timeProvider);

        if (role == EvaluatorRoles.Self)
            appraisal.LockSelf(isDeadline: false);
        else if (role == EvaluatorRoles.Manager || role == EvaluatorRoles.Peer || role == EvaluatorRoles.Subordinate)
            appraisal.LockThreeSixty(isDeadline: false);
        else if (role == EvaluatorRoles.Appraisal)
            appraisal.LockAppraisal(isDeadline: false);

        _uow.Perf.Appraisals.Update(appraisal);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(EvaluationResponseMsg.Submitted);
    }
}
