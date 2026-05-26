namespace EPMS.Domain.Interface.Irepo.App
{
    public interface IAppModule
    {
        ISystemSettingsRepository SystemSettings { get; }
        INotificationRepository Notifications { get; }
    }
}
