using AutoMapper;
using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
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
        private IKPIMasterRepository? _kpiMasterRepository;
        private IPositionKPIRepository? _positionKPIRepository;
        private IPositionKPIHistoryRepository? _positionKPIHistoryRepository;

        public IAppraisalRepository Appraisals =>
            _perfAppraisalRepository ??= serviceProvider.GetRequiredService<IAppraisalRepository>();

        public IRatingScaleRepository RatingScales =>
            _perfRatingScaleRepository ??= serviceProvider.GetRequiredService<IRatingScaleRepository>();

        public IKPIWeightPriorityRepository KPIWeightPriorities =>
            _perfKPIWeightPriorityRepository ??= serviceProvider.GetRequiredService<IKPIWeightPriorityRepository>();

        public IKPIMasterRepository KPIMasters =>
            _kpiMasterRepository ??= serviceProvider.GetRequiredService<IKPIMasterRepository>();

        public IPositionKPIRepository PositionKPIs =>
            _positionKPIRepository ??= serviceProvider.GetRequiredService<IPositionKPIRepository>();

        public IPositionKPIHistoryRepository PositionKPIHistories =>
            _positionKPIHistoryRepository ??= serviceProvider.GetRequiredService<IPositionKPIHistoryRepository>();
    }
}
