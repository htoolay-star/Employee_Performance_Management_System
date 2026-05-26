namespace EPMS.Domain.Interface.IService.App
{
    public interface ISystemSettingsService
    {
        Task<string?> GetSettingValueAsync(string key);

        Task<string> GetDefaultPasswordAsync();

        Task UpdateSettingAsync(string key, string newValue);

        Task UpdateDefaultPasswordAsync(string newPlainPassword);

        Task<long?> GetAdminPositionIdAsync();

        Task SetAdminPositionIdAsync(long positionId);
    }
}
