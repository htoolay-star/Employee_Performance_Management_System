using EPMS.Domain.Entities.Performance;

public interface IEmployeeKPIResultRepository
{
    Task AddAsync(EmployeeKPIResult entity);

    Task<List<EmployeeKPIResult>> GetByEmployeeAsync(
        long employeeId,
        long cycleId);

    Task SaveChangesAsync();
}