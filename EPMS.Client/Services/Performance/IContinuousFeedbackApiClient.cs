using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IContinuousFeedbackApiClient
{
    [Get("/api/performance/continuous-feedbacks")]
    Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetAllAsync();

    [Get("/api/performance/continuous-feedbacks/{id}")]
    Task<SuccessResponse<ContinuousFeedbackDto>> GetByIdAsync(long id);

    [Get("/api/performance/continuous-feedbacks/employee/{employeeId}")]
    Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetByEmployeeIdAsync(long employeeId);

    [Get("/api/performance/continuous-feedbacks/by-user/{userId}")]
    Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetByUserIdAsync(long userId);

    [Post("/api/performance/continuous-feedbacks")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateContinuousFeedbackDto dto);

    [Put("/api/performance/continuous-feedbacks/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateContinuousFeedbackDto dto);

    [Delete("/api/performance/continuous-feedbacks/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
