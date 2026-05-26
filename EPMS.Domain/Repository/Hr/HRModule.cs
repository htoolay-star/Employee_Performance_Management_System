using EPMS.Domain.Interface.Irepo.Hr;
using Microsoft.Extensions.DependencyInjection;

namespace EPMS.Domain.Repository.Hr
{
    public class HRModule(IServiceProvider serviceProvider) : IHRModule
    {
        private IDepartmentRepository? _departments;
        private ITeamRepository? _teams;
        private ILevelRepository? _levels;
        private IPositionRepository? _positions;

        public IDepartmentRepository Departments =>
        _departments ??= serviceProvider.GetRequiredService<IDepartmentRepository>();

        public ITeamRepository Teams =>
        _teams ??= serviceProvider.GetRequiredService<ITeamRepository>();

        public ILevelRepository Levels =>
        _levels ??= serviceProvider.GetRequiredService<ILevelRepository>();

        public IPositionRepository Positions =>
        _positions ??= serviceProvider.GetRequiredService<IPositionRepository>();
    }
}
