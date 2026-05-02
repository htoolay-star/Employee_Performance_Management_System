using EPMS.Domain.Data;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Interface.Irepo.Info;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Info
{
    public class InfoModule(IServiceProvider serviceProvider) : IInfoModule
    {
        private IEmployeeProfileRepository? _profiles;

        public IEmployeeProfileRepository EmployeeProfiles =>
        _profiles ??= serviceProvider.GetRequiredService<IEmployeeProfileRepository>();
    }
}
