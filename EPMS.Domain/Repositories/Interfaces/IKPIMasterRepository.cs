using EPMS.Domain.Entities.Performance;

public interface IKPIMasterRepository
{
    Task<List<KPIMaster>> GetAllAsync();

    Task<KPIMaster?> GetByIdAsync(long id);

    Task AddAsync(KPIMaster entity);

    Task SaveChangesAsync();
}