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

        public ISystemSettingsRepository SystemSettings =>
        _systemSettings ??= serviceProvider.GetRequiredService<ISystemSettingsRepository>();
    }
}
