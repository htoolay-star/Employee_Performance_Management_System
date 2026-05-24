using EPMS.Shared.DTOs.Common;

namespace EPMS.Client.Services.Auth;

public class CurrentUserPermissionService
{
    private readonly IAuthApiClient _apiClient;
    private List<string>? _cachedPermissions;
    private DateTime _lastFetched;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public CurrentUserPermissionService(IAuthApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<bool> HasPermissionAsync(string permissionCode)
    {
        await EnsureLoadedAsync();
        return _cachedPermissions?.Contains(permissionCode) ?? false;
    }

    public async Task<bool> HasAnyPermissionAsync(params string[] permissionCodes)
    {
        await EnsureLoadedAsync();
        return permissionCodes.Any(pc => _cachedPermissions?.Contains(pc) ?? false);
    }

    public async Task<List<string>> GetPermissionsAsync()
    {
        await EnsureLoadedAsync();
        return _cachedPermissions ?? new();
    }

    public void InvalidateCache()
    {
        _cachedPermissions = null;
        _lastFetched = DateTime.MinValue;
    }

    private async Task EnsureLoadedAsync()
    {
        if (_cachedPermissions != null && DateTime.UtcNow - _lastFetched < CacheDuration)
            return;

        try
        {
            var response = await _apiClient.GetMyPermissionsAsync();
            _cachedPermissions = response.Success ? response.Data ?? new() : new();
        }
        catch
        {
            _cachedPermissions ??= new();
        }
        _lastFetched = DateTime.UtcNow;
    }
}
