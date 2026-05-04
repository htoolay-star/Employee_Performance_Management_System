using EPMS.Domain.Data;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Interface.Irepo.Hr;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Hr
{
    public class HRModule(IServiceProvider serviceProvider) : IHRModule
    {
        private IDepartmentRepository? _departments;
        private ITeamRepository? _teams;
        private ILevelRepository? _levels;
        private IPositionRepository? _positions;
        private IRatingScaleRepository? _ratingScales;

        public IDepartmentRepository Departments =>
        _departments ??= serviceProvider.GetRequiredService<IDepartmentRepository>();

        public ITeamRepository Teams =>
        _teams ??= serviceProvider.GetRequiredService<ITeamRepository>();

        public ILevelRepository Levels =>
        _levels ??= serviceProvider.GetRequiredService<ILevelRepository>();

        public IPositionRepository Positions =>
        _positions ??= serviceProvider.GetRequiredService<IPositionRepository>();

        public IRatingScaleRepository RatingScales =>
        _ratingScales ??= serviceProvider.GetRequiredService<IRatingScaleRepository>();
    }
}
