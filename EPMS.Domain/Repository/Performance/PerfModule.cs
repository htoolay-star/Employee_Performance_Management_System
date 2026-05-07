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
        private IRatingScaleRepository? _perfRatingScaleRepository;
        private IKPIWeightPriorityRepository? _perfKPIWeightPriorityRepository;
        private IAppraisalCycleRepository? _perfAppraisalCycleRepository;
        private IKPIMasterRepository? _perfKPIMasterRepository;

        public IAppraisalRepository Appraisals =>
            _perfAppraisalRepository ??= serviceProvider.GetRequiredService<IAppraisalRepository>();

        public IRatingScaleRepository RatingScales =>
            _perfRatingScaleRepository ??= serviceProvider.GetRequiredService<IRatingScaleRepository>();

        public IKPIWeightPriorityRepository KPIWeightPriorities =>
            _perfKPIWeightPriorityRepository ??= serviceProvider.GetRequiredService<IKPIWeightPriorityRepository>();

        public IAppraisalCycleRepository AppraisalCycles =>
            _perfAppraisalCycleRepository ??= serviceProvider.GetRequiredService<IAppraisalCycleRepository>();

        public IKPIMasterRepository KPIMasters =>
            _perfKPIMasterRepository ??= serviceProvider.GetRequiredService<IKPIMasterRepository>();
    }
}
