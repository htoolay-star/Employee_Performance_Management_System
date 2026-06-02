using EPMS.Domain.Data;
using EPMS.Domain.Interface.IService;
using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.DTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.ReportDTOs;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;
    private readonly IExcelService _excelService;

    public ReportService(AppDbContext context, IExcelService excelService)
    {
        _context = context;
        _excelService = excelService;
    }

    public async Task<IEnumerable<EmployeePerformanceSummaryDto>> GetEmployeePerformanceSummaryAsync()
    {
        return await _context.EmployeeProfiles
            .IgnoreQueryFilters()
            .Include(p => p.Employment).ThenInclude(e => e.Department)
            .Include(p => p.Employment).ThenInclude(e => e.Position)
            .Include(p => p.EmployeeKPIs)
            .Where(p => !p.IsDeleted)
            .Select(p => new EmployeePerformanceSummaryDto
            {
                StaffNo = p.StaffNo,
                StaffName = p.StaffName,
                EmailAddress = p.EmailAddress,
                DepartmentName = p.Employment != null && p.Employment.Department != null
                    ? p.Employment.Department.Name
                    : "N/A",
                PositionTitle = p.Employment != null && p.Employment.Position != null
                    ? p.Employment.Position.Name
                    : "N/A",
                EmploymentStatus = p.Employment != null ? p.Employment.EmploymentStatus : "Unknown",
                FinalAppraisalScore = p.EmployeeKPIs != null && p.EmployeeKPIs.Any()
                    ? (double)p.EmployeeKPIs.Sum(k => k.Weightage)
                    : 0,
                PerformanceGrade = p.EmployeeKPIs != null && p.EmployeeKPIs.Any()
                    ? (p.EmployeeKPIs.Sum(k => k.Weightage) >= 90 ? "A"
                       : p.EmployeeKPIs.Sum(k => k.Weightage) >= 75 ? "B"
                       : p.EmployeeKPIs.Sum(k => k.Weightage) >= 50 ? "C" : "D")
                    : "No Grade"
            })
            .ToListAsync();
    }

    public async Task<List<DepartmentPerformanceDto>> GetDepartmentPerformanceAsync(long? cycleId)
    {
        long resolvedCycleId;

        if (cycleId.HasValue)
        {
            resolvedCycleId = cycleId.Value;
        }
        else
        {
            var latest = await _context.AppraisalCycles
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => (long?)c.Id)
                .FirstOrDefaultAsync();

            if (latest == null)
                return new List<DepartmentPerformanceDto>();

            resolvedCycleId = latest.Value;
        }

        var deptEmployees = await _context.EmployeeEmployments
            .IgnoreQueryFilters()
            .Where(e => !e.IsDeleted && e.Profile != null && !e.Profile.IsDeleted)
            .GroupBy(e => new { e.DepartmentId, e.Department.Name, DeptHeadName = e.Department.DeptHead != null ? e.Department.DeptHead.StaffName : null })
            .Select(g => new
            {
                g.Key.DepartmentId,
                g.Key.Name,
                g.Key.DeptHeadName,
                EmployeeCount = g.Count()
            })
            .ToListAsync();

        var deptScores = await _context.Appraisals
            .IgnoreQueryFilters()
            .Where(a => a.CycleId == resolvedCycleId && a.Status == "FINALIZED"
                && a.EmployeeId != null && a.Employee != null
                && a.Employee.Employment != null && !a.Employee.Employment.IsDeleted
                && a.Employee.Employment.Department != null)
            .GroupBy(a => new { a.Employee.Employment.DepartmentId, a.Employee.Employment.Department.Name })
            .Select(g => new
            {
                g.Key.DepartmentId,
                EvaluatedCount = g.Select(a => a.EmployeeId).Distinct().Count(),
                AvgScore = g.Average(a => (double?)a.TotalScore),
                MinScore = g.Min(a => a.TotalScore),
                MaxScore = g.Max(a => a.TotalScore),
                HighCount = g.Count(a => a.TotalScore >= 80),
                LowCount = g.Count(a => a.TotalScore < 60)
            })
            .ToListAsync();

        var scoreMap = deptScores.ToDictionary(s => s.DepartmentId);

        var result = deptEmployees.Select(de =>
        {
            var scores = scoreMap.GetValueOrDefault(de.DepartmentId);
            return new DepartmentPerformanceDto
            {
                DepartmentId = de.DepartmentId,
                DepartmentName = de.Name,
                DeptHeadName = de.DeptHeadName,
                EmployeeCount = de.EmployeeCount,
                EvaluatedCount = scores?.EvaluatedCount ?? 0,
                AvgTotalScore = scores?.AvgScore,
                MinScore = scores?.MinScore,
                MaxScore = scores?.MaxScore,
                HighPerformerCount = scores?.HighCount ?? 0,
                LowPerformerCount = scores?.LowCount ?? 0
            };
        }).ToList();

        return result;
    }

    public async Task<List<HighLowPerformerDto>> GetHighLowPerformersAsync(long? cycleId, int topCount, bool isHigh)
    {
        long resolvedCycleId;

        if (cycleId.HasValue)
        {
            resolvedCycleId = cycleId.Value;
        }
        else
        {
            var latest = await _context.AppraisalCycles
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => (long?)c.Id)
                .FirstOrDefaultAsync();

            if (latest == null)
                return new List<HighLowPerformerDto>();

            resolvedCycleId = latest.Value;
        }

        var query = _context.Appraisals
            .IgnoreQueryFilters()
            .Where(a => a.CycleId == resolvedCycleId && a.Status == "FINALIZED"
                && a.EmployeeId != null && a.TotalScore != null)
            .Include(a => a.Employee).ThenInclude(e => e.Employment).ThenInclude(e => e.Department)
            .Include(a => a.Employee).ThenInclude(e => e.Employment).ThenInclude(e => e.Position)
            .Include(a => a.Cycle)
            .AsQueryable();

        query = isHigh
            ? query.OrderByDescending(a => a.TotalScore)
            : query.OrderBy(a => a.TotalScore);

        var appraisals = await query.Take(topCount).ToListAsync();

        var employeeIds = appraisals.Select(a => a.EmployeeId!.Value).ToList();

        var pipStatuses = await _context.PIPs
            .IgnoreQueryFilters()
            .Where(p => employeeIds.Contains(p.EmployeeId)
                && (p.Status == "OPEN" || p.Status == "EXTENDED"))
            .GroupBy(p => p.EmployeeId)
            .Select(g => new { EmployeeId = g.Key, Status = g.First().Status })
            .ToListAsync();

        var pipMap = pipStatuses.ToDictionary(p => p.EmployeeId, p => p.Status);

        var result = appraisals.Select((a, i) => new HighLowPerformerDto
        {
            Rank = i + 1,
            EmployeeId = a.EmployeeId!.Value,
            StaffNo = a.Employee!.StaffNo,
            StaffName = a.Employee.StaffName,
            DepartmentName = a.Employee.Employment?.Department?.Name,
            PositionName = a.Employee.Employment?.Position?.Name,
            TotalScore = a.TotalScore,
            RatingLabel = a.RatingLabel,
            CycleName = a.Cycle?.Name,
            PIPStatus = pipMap.GetValueOrDefault(a.EmployeeId!.Value)
        }).ToList();

        return result;
    }

    public async Task<List<PromotionRecommendationDto>> GetPromotionRecommendationsAsync(long? cycleId)
    {
        long resolvedCycleId;

        if (cycleId.HasValue)
        {
            resolvedCycleId = cycleId.Value;
        }
        else
        {
            var latest = await _context.AppraisalCycles
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => (long?)c.Id)
                .FirstOrDefaultAsync();

            if (latest == null)
                return new List<PromotionRecommendationDto>();

            resolvedCycleId = latest.Value;
        }

        var appraisals = await _context.Appraisals
            .IgnoreQueryFilters()
            .Where(a => a.CycleId == resolvedCycleId && a.Status == "FINALIZED"
                && a.EmployeeId != null && a.TotalScore != null
                && a.FinalRatingId != null
                && a.FinalRating != null
                && a.FinalRating.PromotionEligibility != null
                && a.FinalRating.PromotionEligibility != "")
            .Include(a => a.Employee).ThenInclude(e => e.Employment).ThenInclude(e => e.Department)
            .Include(a => a.Employee).ThenInclude(e => e.Employment).ThenInclude(e => e.Position).ThenInclude(p => p.Level)
            .Include(a => a.Cycle)
            .ToListAsync();

        var result = appraisals
            .Select(a => new PromotionRecommendationDto
            {
                EmployeeId = a.EmployeeId!.Value,
                StaffNo = a.Employee!.StaffNo,
                StaffName = a.Employee.StaffName,
                DepartmentName = a.Employee.Employment?.Department?.Name,
                PositionName = a.Employee.Employment?.Position?.Name,
                LevelName = a.Employee.Employment?.Position?.Level?.Name,
                TotalScore = a.TotalScore,
                RatingLabel = a.RatingLabel,
                PromotionEligibility = a.FinalRating?.PromotionEligibility,
                CycleName = a.Cycle?.Name,
                TenureMonths = a.Employee.Employment?.DateOfAppointment != null
                    ? (int?)((DateTimeOffset.UtcNow - a.Employee.Employment.DateOfAppointment.Value.ToDateTime(TimeOnly.MinValue).ToUniversalTime()).TotalDays / 30.44)
                    : null
            })
            .OrderByDescending(d => d.TotalScore)
            .ToList();

        return result;
    }

    public async Task<List<EmployeeSummaryReportDto>> GetEmployeeSummaryReportAsync(long? cycleId, long? employeeId)
    {
        var query = _context.Appraisals
            .IgnoreQueryFilters()
            .Where(a => a.EmployeeId != null)
            .Include(a => a.Employee).ThenInclude(e => e.Employment).ThenInclude(e => e.Department)
            .Include(a => a.Employee).ThenInclude(e => e.Employment).ThenInclude(e => e.Position)
            .Include(a => a.Cycle)
            .AsQueryable();

        if (cycleId.HasValue)
            query = query.Where(a => a.CycleId == cycleId.Value);

        if (employeeId.HasValue)
            query = query.Where(a => a.EmployeeId == employeeId.Value);

        var result = await query
            .OrderByDescending(a => a.Cycle.CreatedAt)
            .ThenBy(a => a.Employee!.StaffName)
            .Select(a => new EmployeeSummaryReportDto
            {
                EmployeeId = a.EmployeeId!.Value,
                StaffNo = a.Employee!.StaffNo,
                StaffName = a.Employee.StaffName,
                DepartmentName = a.Employee.Employment != null ? a.Employee.Employment.Department.Name : null,
                PositionName = a.Employee.Employment != null ? a.Employee.Employment.Position.Name : null,
                CycleName = a.Cycle.Name,
                TotalScore = a.TotalScore,
                KpiScore = a.KpiScore,
                SelfScore = a.SelfScore,
                ThreeSixtyScore = a.ThreeSixtyScore,
                AppraisalScore = a.AppraisalScore,
                RatingLabel = a.RatingLabel,
                Status = a.Status,
                FinalizedDate = a.FinalizedDate
            })
            .ToListAsync();

        return result;
    }

    public async Task<SuccessResponse<byte[]>> ExportEmployeeSummaryExcelAsync(long? cycleId, long? employeeId)
    {
        var data = await GetEmployeeSummaryReportAsync(cycleId, employeeId);
        return await _excelService.ExportAsync(data, "Employee Summary");
    }

    public async Task<SuccessResponse<byte[]>> ExportDepartmentComparisonExcelAsync(long? cycleId)
    {
        var data = await GetDepartmentPerformanceAsync(cycleId);
        return await _excelService.ExportAsync(data, "Department Comparison");
    }

    public async Task<SuccessResponse<byte[]>> ExportHighLowPerformersExcelAsync(long? cycleId, int topCount, bool isHigh)
    {
        var data = await GetHighLowPerformersAsync(cycleId, topCount, isHigh);
        var sheetName = isHigh ? "High Performers" : "Low Performers";
        return await _excelService.ExportAsync(data, sheetName);
    }

    public async Task<SuccessResponse<byte[]>> ExportPromotionRecommendationsExcelAsync(long? cycleId)
    {
        var data = await GetPromotionRecommendationsAsync(cycleId);
        return await _excelService.ExportAsync(data, "Promotion Recommendations");
    }
}
