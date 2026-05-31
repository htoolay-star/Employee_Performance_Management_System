using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.ReportDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[ApiController]
[Route("api/performance-reports")]
[Authorize]
public class PerformanceReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public PerformanceReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("department-comparison")]
    public async Task<ActionResult<SuccessResponse<List<DepartmentPerformanceDto>>>> GetDepartmentComparison([FromQuery] long? cycleId)
    {
        var data = await _reportService.GetDepartmentPerformanceAsync(cycleId);
        return Ok(SuccessResponse<List<DepartmentPerformanceDto>>.Ok(data, "Department comparison data retrieved."));
    }

    [HttpGet("high-low-performers")]
    public async Task<ActionResult<SuccessResponse<List<HighLowPerformerDto>>>> GetHighLowPerformers(
        [FromQuery] long? cycleId, [FromQuery] int topCount = 10, [FromQuery] bool isHigh = true)
    {
        var data = await _reportService.GetHighLowPerformersAsync(cycleId, topCount, isHigh);
        return Ok(SuccessResponse<List<HighLowPerformerDto>>.Ok(data, "Performer data retrieved."));
    }

    [HttpGet("promotion-recommendations")]
    public async Task<ActionResult<SuccessResponse<List<PromotionRecommendationDto>>>> GetPromotionRecommendations([FromQuery] long? cycleId)
    {
        var data = await _reportService.GetPromotionRecommendationsAsync(cycleId);
        return Ok(SuccessResponse<List<PromotionRecommendationDto>>.Ok(data, "Promotion recommendations retrieved."));
    }

    [HttpGet("employee-summary")]
    public async Task<ActionResult<SuccessResponse<List<EmployeeSummaryReportDto>>>> GetEmployeeSummary(
        [FromQuery] long? cycleId, [FromQuery] long? employeeId)
    {
        var data = await _reportService.GetEmployeeSummaryReportAsync(cycleId, employeeId);
        return Ok(SuccessResponse<List<EmployeeSummaryReportDto>>.Ok(data, "Employee summary data retrieved."));
    }

    [HttpGet("employee-summary-excel")]
    public async Task<IActionResult> GetEmployeeSummaryExcel(
        [FromQuery] long? cycleId, [FromQuery] long? employeeId)
    {
        var result = await _reportService.ExportEmployeeSummaryExcelAsync(cycleId, employeeId);
        if (!result.Success)
            return BadRequest(result);

        return File(result.Data,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"EmployeeSummary_{DateTime.UtcNow:yyyyMMdd}.xlsx");
    }

    [HttpGet("department-comparison-excel")]
    public async Task<IActionResult> GetDepartmentComparisonExcel([FromQuery] long? cycleId)
    {
        var result = await _reportService.ExportDepartmentComparisonExcelAsync(cycleId);
        if (!result.Success)
            return BadRequest(result);

        return File(result.Data,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"DepartmentComparison_{DateTime.UtcNow:yyyyMMdd}.xlsx");
    }

    [HttpGet("high-low-performers-excel")]
    public async Task<IActionResult> GetHighLowPerformersExcel(
        [FromQuery] long? cycleId, [FromQuery] int topCount = 10, [FromQuery] bool isHigh = true)
    {
        var result = await _reportService.ExportHighLowPerformersExcelAsync(cycleId, topCount, isHigh);
        if (!result.Success)
            return BadRequest(result);

        var prefix = isHigh ? "HighPerformers" : "LowPerformers";
        return File(result.Data,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{prefix}_{DateTime.UtcNow:yyyyMMdd}.xlsx");
    }

    [HttpGet("promotion-recommendations-excel")]
    public async Task<IActionResult> GetPromotionRecommendationsExcel([FromQuery] long? cycleId)
    {
        var result = await _reportService.ExportPromotionRecommendationsExcelAsync(cycleId);
        if (!result.Success)
            return BadRequest(result);

        return File(result.Data,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"PromotionRecommendations_{DateTime.UtcNow:yyyyMMdd}.xlsx");
    }
}
