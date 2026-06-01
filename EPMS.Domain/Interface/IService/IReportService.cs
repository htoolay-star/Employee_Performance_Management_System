using EPMS.Shared.DTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.ReportDTOs;

namespace EPMS.Domain.Interface.IService;

public interface IReportService
{
    Task<IEnumerable<EmployeePerformanceSummaryDto>> GetEmployeePerformanceSummaryAsync();
    Task<List<DepartmentPerformanceDto>> GetDepartmentPerformanceAsync(long? cycleId);
    Task<List<HighLowPerformerDto>> GetHighLowPerformersAsync(long? cycleId, int topCount, bool isHigh);
    Task<List<PromotionRecommendationDto>> GetPromotionRecommendationsAsync(long? cycleId);
    Task<List<EmployeeSummaryReportDto>> GetEmployeeSummaryReportAsync(long? cycleId, long? employeeId);
    Task<SuccessResponse<byte[]>> ExportEmployeeSummaryExcelAsync(long? cycleId, long? employeeId);
    Task<SuccessResponse<byte[]>> ExportDepartmentComparisonExcelAsync(long? cycleId);
    Task<SuccessResponse<byte[]>> ExportHighLowPerformersExcelAsync(long? cycleId, int topCount, bool isHigh);
    Task<SuccessResponse<byte[]>> ExportPromotionRecommendationsExcelAsync(long? cycleId);
}
