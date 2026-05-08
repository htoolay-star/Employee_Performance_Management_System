using EPMS.Domain.Interface.Irepo.App;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.App
{
public class AppModule(IServiceProvider serviceProvider) : IAppModule
{
    private ISystemSettingsRepository? _systemSettings;
    private INotificationRepository? _notifications;

    public ISystemSettingsRepository SystemSettings =>
        _systemSettings ??= serviceProvider.GetRequiredService<ISystemSettingsRepository>();

    public INotificationRepository Notifications =>
        _notifications ??= serviceProvider.GetRequiredService<INotificationRepository>();
}
}
