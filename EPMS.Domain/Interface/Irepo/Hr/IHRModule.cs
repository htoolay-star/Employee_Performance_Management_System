namespace EPMS.Domain.Interface.Irepo.Hr
{
    public interface IHRModule
    {
        IDepartmentRepository Departments { get; }
        ITeamRepository Teams { get; }
        ILevelRepository Levels { get; }
        IPositionRepository Positions { get; }
    }
}
