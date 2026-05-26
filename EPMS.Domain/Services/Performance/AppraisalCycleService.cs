using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Performance;

public class AppraisalCycleService : IAppraisalCycleService
{
    private readonly IUnitOfWork _uow;

    public AppraisalCycleService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<AppraisalCycleDto>>> GetAllAsync()
    {
        var cycles = await _uow.Perf.AppraisalCycles.GetAllAsync();
        var dtos = cycles.Adapt<IEnumerable<AppraisalCycleDto>>();
        return SuccessResponse<IEnumerable<AppraisalCycleDto>>.Ok(dtos, AppraisalCycleMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<AppraisalCycleDto>>> GetActiveCyclesAsync()
    {
        var cycles = await _uow.Perf.AppraisalCycles.GetActiveCyclesAsync();
        var dtos = cycles.Adapt<IEnumerable<AppraisalCycleDto>>();
        return SuccessResponse<IEnumerable<AppraisalCycleDto>>.Ok(dtos, AppraisalCycleMsg.RetrievedActive);
    }

    public async Task<SuccessResponse<AppraisalCycleDto>> GetByIdAsync(long id)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);

        if (cycle == null)
            return SuccessResponse<AppraisalCycleDto>.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);

        var dto = cycle.Adapt<AppraisalCycleDto>();
        return SuccessResponse<AppraisalCycleDto>.Ok(dto, AppraisalCycleMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateAppraisalCycleDto dto)
    {
        var existing = await _uow.Perf.AppraisalCycles.GetByYearAndTypeAsync(dto.YearLabel, dto.AppraisalType);
        if (existing != null)
        {
            return SuccessResponse<long>.Fail(
                string.Format(AppraisalCycleMsg.DuplicateCycle, dto.YearLabel, dto.AppraisalType),
                ErrorType.Conflict);
        }

        if (!dto.EvaluationStartDate.HasValue || !dto.EvaluationEndDate.HasValue)
        {
            var (evalStart, evalEnd) = CalculateEvaluationDates(
                dto.AppraisalType, dto.CalendarType, dto.YearLabel);
            dto.EvaluationStartDate ??= evalStart;
            dto.EvaluationEndDate ??= evalEnd;
        }

        var upperType = dto.AppraisalType.Trim().ToUpperInvariant();
        var (minDays, maxDays) = upperType switch
        {
            AppraisalConstants.AppraisalTypes.Monthly => (20, 31),
            AppraisalConstants.AppraisalTypes.Quarterly => (60, 92),
            AppraisalConstants.AppraisalTypes.SemiAnnual => (120, 184),
            AppraisalConstants.AppraisalTypes.Annual => (300, 366),
            _ => (300, 366)
        };
        var actualDays = dto.EvaluationEndDate.Value.DayNumber - dto.EvaluationStartDate.Value.DayNumber;
        if (actualDays < minDays)
        {
            return SuccessResponse<long>.Fail(
                $"Evaluation period ({actualDays} days) is below minimum of {minDays} days for {upperType}.",
                ErrorType.Validation);
        }
        if (actualDays > maxDays)
        {
            return SuccessResponse<long>.Fail(
                $"Evaluation period ({actualDays} days) exceeds maximum of {maxDays} days for {upperType}.",
                ErrorType.Validation);
        }

        if (!IsEvaluationWithinYear(dto.CalendarType, dto.YearLabel, dto.EvaluationStartDate.Value, dto.EvaluationEndDate.Value, out var yearError))
        {
            return SuccessResponse<long>.Fail(yearError!, ErrorType.Validation);
        }

        if (dto.WindowStartDate < dto.EvaluationEndDate.Value)
        {
            return SuccessResponse<long>.Fail(
                "Window start date must be on or after evaluation end date.",
                ErrorType.Validation);
        }

        var cycle = new AppraisalCycle(
            dto.Name,
            dto.AppraisalType,
            dto.CalendarType,
            dto.YearLabel,
            dto.EvaluationStartDate.Value,
            dto.EvaluationEndDate.Value,
            dto.WindowStartDate,
            dto.WindowEndDate,
            dto.KpiWeight,
            dto.SelfWeight,
            dto.ThreeSixtyWeight,
            dto.AppraisalWeight
        );

        try
        {
            if (dto.SelfReviewStartDate.HasValue && dto.SelfReviewDeadline.HasValue)
                cycle.ConfigureSelfReviewWindow(dto.SelfReviewStartDate.Value, dto.SelfReviewDeadline.Value);
        }
        catch (ArgumentException ex)
        {
            return SuccessResponse<long>.Fail(ex.Message, ErrorType.Validation);
        }

        try
        {
            if (dto.AppraisalReviewStartDate.HasValue && dto.AppraisalReviewDeadline.HasValue)
                cycle.ConfigureAppraisalReviewWindow(dto.AppraisalReviewStartDate.Value, dto.AppraisalReviewDeadline.Value);
        }
        catch (ArgumentException ex)
        {
            return SuccessResponse<long>.Fail(ex.Message, ErrorType.Validation);
        }

        try
        {
            if (dto.KpiReviewStartDate.HasValue && dto.KpiReviewDeadline.HasValue)
                cycle.ConfigureKpiReviewWindow(dto.KpiReviewStartDate.Value, dto.KpiReviewDeadline.Value);
        }
        catch (ArgumentException ex)
        {
            return SuccessResponse<long>.Fail(ex.Message, ErrorType.Validation);
        }

        try
        {
            if (dto.ThreeSixtyReviewStartDate.HasValue && dto.ThreeSixtyReviewDeadline.HasValue)
                cycle.ConfigureThreeSixtyReviewWindow(dto.ThreeSixtyReviewStartDate.Value, dto.ThreeSixtyReviewDeadline.Value);
        }
        catch (ArgumentException ex)
        {
            return SuccessResponse<long>.Fail(ex.Message, ErrorType.Validation);
        }

        _uow.Perf.AppraisalCycles.Add(cycle);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(cycle.Id, AppraisalCycleMsg.Created);
    }

    private static bool IsEvaluationWithinYear(string calendarType, string yearLabel, DateOnly start, DateOnly end, out string? error)
    {
        error = null;
        var upperCal = calendarType.Trim().ToUpperInvariant();
        var label = yearLabel.Trim();

        if (!int.TryParse(label[..Math.Min(4, label.Length)], out var year))
        {
            error = $"Invalid year label: '{label}'.";
            return false;
        }

        DateOnly yearStart, yearEnd;
        if (upperCal == AppraisalConstants.CalendarTypes.FiscalYear)
        {
            yearStart = new DateOnly(year, 4, 1);
            yearEnd = new DateOnly(year + 1, 3, 31);
        }
        else
        {
            yearStart = new DateOnly(year, 1, 1);
            yearEnd = new DateOnly(year, 12, 31);
        }

        if (start < yearStart || end > yearEnd)
        {
            error = $"Evaluation dates must fall within the allowed range for '{yearLabel}': {yearStart:dd/MM/yyyy} to {yearEnd:dd/MM/yyyy}.";
            return false;
        }

        return true;
    }

    private static (DateOnly Start, DateOnly End) CalculateEvaluationDates(
        string appraisalType, string calendarType, string yearLabel)
    {
        var upperType = appraisalType.Trim().ToUpperInvariant();
        var upperCal = calendarType.Trim().ToUpperInvariant();
        var year = int.Parse(yearLabel[..4]);

        if (upperCal == AppraisalConstants.CalendarTypes.FiscalYear)
        {
            return upperType switch
            {
                AppraisalConstants.AppraisalTypes.Annual => (new DateOnly(year, 4, 1), new DateOnly(year + 1, 3, 31)),
                AppraisalConstants.AppraisalTypes.SemiAnnual => (new DateOnly(year, 4, 1), new DateOnly(year, 9, 30)),
                AppraisalConstants.AppraisalTypes.Quarterly => (new DateOnly(year, 4, 1), new DateOnly(year, 6, 30)),
                AppraisalConstants.AppraisalTypes.Monthly => (new DateOnly(year, 4, 1), new DateOnly(year, 4, 30)),
                _ => (new DateOnly(year, 4, 1), new DateOnly(year + 1, 3, 31))
            };
        }

        // Standard calendar
        return upperType switch
        {
            AppraisalConstants.AppraisalTypes.Annual => (new DateOnly(year, 1, 1), new DateOnly(year, 12, 31)),
            AppraisalConstants.AppraisalTypes.SemiAnnual => (new DateOnly(year, 1, 1), new DateOnly(year, 6, 30)),
            AppraisalConstants.AppraisalTypes.Quarterly => (new DateOnly(year, 1, 1), new DateOnly(year, 3, 31)),
            AppraisalConstants.AppraisalTypes.Monthly => (new DateOnly(year, 1, 1), new DateOnly(year, 1, 31)),
            _ => (new DateOnly(year, 1, 1), new DateOnly(year, 12, 31))
        };
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateAppraisalCycleDto dto)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);
        if (cycle == null)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);
        }

        if (cycle.IsLocked)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.AlreadyLocked, ErrorType.Validation);
        }

        if (dto.WindowStartDate < dto.EvaluationEndDate)
        {
            return SuccessResponse.Fail(
                "Window start date must be on or after evaluation end date.",
                ErrorType.Validation);
        }

        var upperType = dto.AppraisalType.Trim().ToUpperInvariant();
        var (minDays, maxDays) = upperType switch
        {
            AppraisalConstants.AppraisalTypes.Monthly => (20, 31),
            AppraisalConstants.AppraisalTypes.Quarterly => (60, 92),
            AppraisalConstants.AppraisalTypes.SemiAnnual => (120, 184),
            AppraisalConstants.AppraisalTypes.Annual => (300, 366),
            _ => (300, 366)
        };
        var actualDays = dto.EvaluationEndDate.DayNumber - dto.EvaluationStartDate.DayNumber;
        if (actualDays < minDays)
        {
            return SuccessResponse.Fail(
                $"Evaluation period ({actualDays} days) is below minimum of {minDays} days for {upperType}.",
                ErrorType.Validation);
        }
        if (actualDays > maxDays)
        {
            return SuccessResponse.Fail(
                $"Evaluation period ({actualDays} days) exceeds maximum of {maxDays} days for {upperType}.",
                ErrorType.Validation);
        }

        if (!IsEvaluationWithinYear(dto.CalendarType, dto.YearLabel, dto.EvaluationStartDate, dto.EvaluationEndDate, out var yearRangeMsg))
        {
            return SuccessResponse.Fail(yearRangeMsg!, ErrorType.Validation);
        }

        cycle.Update(dto.Name, dto.AppraisalType, dto.CalendarType, dto.YearLabel,
                     dto.EvaluationStartDate, dto.EvaluationEndDate,
                     dto.WindowStartDate, dto.WindowEndDate,
                     dto.KpiWeight, dto.SelfWeight, dto.ThreeSixtyWeight,
                     dto.AppraisalWeight);

        try
        {
            if (dto.SelfReviewStartDate.HasValue && dto.SelfReviewDeadline.HasValue)
                cycle.ConfigureSelfReviewWindow(dto.SelfReviewStartDate.Value, dto.SelfReviewDeadline.Value);
        }
        catch (ArgumentException ex)
        {
            return SuccessResponse.Fail(ex.Message, ErrorType.Validation);
        }

        try
        {
            if (dto.AppraisalReviewStartDate.HasValue && dto.AppraisalReviewDeadline.HasValue)
                cycle.ConfigureAppraisalReviewWindow(dto.AppraisalReviewStartDate.Value, dto.AppraisalReviewDeadline.Value);
        }
        catch (ArgumentException ex)
        {
            return SuccessResponse.Fail(ex.Message, ErrorType.Validation);
        }

        try
        {
            if (dto.KpiReviewStartDate.HasValue && dto.KpiReviewDeadline.HasValue)
                cycle.ConfigureKpiReviewWindow(dto.KpiReviewStartDate.Value, dto.KpiReviewDeadline.Value);
        }
        catch (ArgumentException ex)
        {
            return SuccessResponse.Fail(ex.Message, ErrorType.Validation);
        }

        try
        {
            if (dto.ThreeSixtyReviewStartDate.HasValue && dto.ThreeSixtyReviewDeadline.HasValue)
                cycle.ConfigureThreeSixtyReviewWindow(dto.ThreeSixtyReviewStartDate.Value, dto.ThreeSixtyReviewDeadline.Value);
        }
        catch (ArgumentException ex)
        {
            return SuccessResponse.Fail(ex.Message, ErrorType.Validation);
        }

        _uow.Perf.AppraisalCycles.Update(cycle);
        await _uow.CompleteAsync();

        if (dto.IsActive.HasValue)
        {
            if (dto.IsActive.Value)
            {
                cycle.Reactivate();
            }
            else cycle.Deactivate();
            await _uow.CompleteAsync();
        }

        return SuccessResponse.Ok(AppraisalCycleMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);
        if (cycle == null)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);
        }

        if (cycle.IsLocked)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.CannotDeleteLocked, ErrorType.Validation);
        }

        _uow.Perf.AppraisalCycles.Delete(cycle);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(AppraisalCycleMsg.Deleted);
    }

    public async Task<SuccessResponse> RestoreAsync(long id)
    {
        var entity = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);
        if (entity == null)
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);
        if (!entity.IsDeleted)
            return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = null;
        _uow.Perf.AppraisalCycles.Update(entity);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalCycleMsg.Updated);
    }

}