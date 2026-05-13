using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.App;

namespace EPMS.Domain.Services.App;

public class CurrentEmployeeContextService : ICurrentEmployeeContextService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _uow;

    private EmployeeProfile? _cachedProfile;
    private EmployeeEmployment? _cachedEmployment;
    private bool _profileLoaded;
    private bool _employmentLoaded;

    public CurrentEmployeeContextService(ICurrentUserService currentUser, IUnitOfWork uow)
    {
        _currentUser = currentUser;
        _uow = uow;
    }

    public bool IsAuthenticated => _currentUser.IsAuthenticated;
    public long? UserId => _currentUser.UserId;

    public async Task<EmployeeProfile?> GetEmployeeProfileAsync(CancellationToken cancellationToken = default)
    {
        if (_profileLoaded) return _cachedProfile;

        _profileLoaded = true;

        if (!_currentUser.IsAuthenticated || _currentUser.UserId == null)
        {
            _cachedProfile = null;
            return null;
        }

        // Optimized: Fetch profile and employment in ONE TRIP
        _cachedProfile = await _uow.Info.EmployeeProfiles.GetByUserIdAsync(_currentUser.UserId.Value);
        
        // If profile exists, check if we can pre-load employment
        if (_cachedProfile != null)
        {
            _cachedEmployment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(_cachedProfile.Id);
            _employmentLoaded = true;
        }

        return _cachedProfile;
    }

    public async Task<EmployeeEmployment?> GetEmploymentAsync(CancellationToken cancellationToken = default)
    {
        if (_employmentLoaded) return _cachedEmployment;

        _employmentLoaded = true;

        var profile = await GetEmployeeProfileAsync(cancellationToken);
        if (profile == null)
        {
            _cachedEmployment = null;
            return null;
        }

        _cachedEmployment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(profile.Id);
        return _cachedEmployment;
    }

    public async Task<long?> GetEmployeeIdAsync(CancellationToken cancellationToken = default)
    {
        var profile = await GetEmployeeProfileAsync(cancellationToken);
        return profile?.Id;
    }

    public async Task<long?> GetPositionIdAsync(CancellationToken cancellationToken = default)
    {
        var employment = await GetEmploymentAsync(cancellationToken);
        return employment?.PositionId;
    }

    public async Task<long?> GetDepartmentIdAsync(CancellationToken cancellationToken = default)
    {
        var employment = await GetEmploymentAsync(cancellationToken);
        return employment?.DepartmentId;
    }

    public async Task<long?> GetTeamIdAsync(CancellationToken cancellationToken = default)
    {
        var employment = await GetEmploymentAsync(cancellationToken);
        return employment?.TeamId;
    }
}
