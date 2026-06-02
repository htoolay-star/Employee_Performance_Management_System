using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.ReportDTOs;
using Refit;
using System.Net.Http;

namespace EPMS.Client.Services.Performance;

public interface IPerformanceReportsApiClient
{
    [Get("/api/performance-reports/department-comparison")]
    Task<SuccessResponse<List<DepartmentPerformanceDto>>> GetDepartmentComparisonAsync(long? cycleId);

    [Get("/api/performance-reports/high-low-performers")]
    Task<SuccessResponse<List<HighLowPerformerDto>>> GetHighLowPerformersAsync(long? cycleId, int topCount, bool isHigh);

    [Get("/api/performance-reports/promotion-recommendations")]
    Task<SuccessResponse<List<PromotionRecommendationDto>>> GetPromotionRecommendationsAsync(long? cycleId);

    [Get("/api/performance-reports/employee-summary")]
    Task<SuccessResponse<List<EmployeeSummaryReportDto>>> GetEmployeeSummaryAsync(long? cycleId, long? employeeId);

    [Get("/api/performance-reports/employee-summary-excel")]
    Task<HttpResponseMessage> GetEmployeeSummaryExcelAsync(long? cycleId, long? employeeId);

    [Get("/api/performance-reports/department-comparison-excel")]
    Task<HttpResponseMessage> GetDepartmentComparisonExcelAsync(long? cycleId);

    [Get("/api/performance-reports/high-low-performers-excel")]
    Task<HttpResponseMessage> GetHighLowPerformersExcelAsync(long? cycleId, int topCount, bool isHigh);

    [Get("/api/performance-reports/promotion-recommendations-excel")]
    Task<HttpResponseMessage> GetPromotionRecommendationsExcelAsync(long? cycleId);

    [Get("/api/performance-reports/department-comparison-pdf")]
    Task<HttpResponseMessage> GetDepartmentComparisonPdfAsync(long? cycleId);

    [Get("/api/performance-reports/high-low-performers-pdf")]
    Task<HttpResponseMessage> GetHighLowPerformersPdfAsync(long? cycleId, int topCount, bool isHigh);

    [Get("/api/performance-reports/promotion-recommendations-pdf")]
    Task<HttpResponseMessage> GetPromotionRecommendationsPdfAsync(long? cycleId);

    [Get("/api/performance-reports/employee-summary-pdf")]
    Task<HttpResponseMessage> GetEmployeeSummaryPdfAsync(long? cycleId, long? employeeId);
}
