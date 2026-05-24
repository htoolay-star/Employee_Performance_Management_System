using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Interface.IService.App;

public interface ICurrentEmployeeContextService
{
    bool IsAuthenticated { get; }
    long? UserId { get; }
    bool IsAdmin { get; }
    Task<EmployeeProfile?> GetEmployeeProfileAsync(CancellationToken cancellationToken = default);
    Task<EmployeeEmployment?> GetEmploymentAsync(CancellationToken cancellationToken = default);
    Task<long?> GetEmployeeIdAsync(CancellationToken cancellationToken = default);
    Task<long?> GetPositionIdAsync(CancellationToken cancellationToken = default);
    Task<long?> GetDepartmentIdAsync(CancellationToken cancellationToken = default);
    Task<long?> GetTeamIdAsync(CancellationToken cancellationToken = default);
}
