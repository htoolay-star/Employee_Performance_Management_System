using AutoMapper;
using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Interface.Irepo.Performance;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Performance
{
    public class PerfModule(IServiceProvider serviceProvider) : IPerfModule
    {
        private IAppraisalRepository? _perfAppraisalRepository;

        public IAppraisalRepository Appraisals =>
        _perfAppraisalRepository ??= serviceProvider.GetRequiredService<IAppraisalRepository>();
    }
}
