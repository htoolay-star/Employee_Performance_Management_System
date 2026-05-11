using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Extensions;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Repository.Base;
using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.Features.Positions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Hr;

public class PositionRepository : GenericRepository<Position>, IPositionRepository
{
    public PositionRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Position>> GetAllWithLevelAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Include(p => p.Level)
            .ToListAsync(cancellationToken);
    }

    public async Task<Position?> GetByIdWithLevelAsync(long id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Position> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
        return await query
            .Include(p => p.Level)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, long? excludePositionId = null, CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim();
        var query = _dbSet.Where(p => p.Name == normalized);
        if (excludePositionId.HasValue)
            query = query.Where(p => p.Id != excludePositionId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, long? excludePositionId = null, CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim().ToUpperInvariant();
        var query = _dbSet.Where(p => p.Code == normalized);
        if (excludePositionId.HasValue)
            query = query.Where(p => p.Id != excludePositionId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(long id)
    {
        return await _dbSet.AnyAsync(p => p.Id == id);
    }

    public async Task<(IEnumerable<PositionGridItemDto> Items, int TotalCount)> GetPagedAsync(PositionQueryParameters parameters, string entitySortColumn, CancellationToken cancellationToken = default)
    {
        IQueryable<Position> baseQuery = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var search = parameters.SearchTerm.Trim();
            baseQuery = baseQuery.Where(p => p.Name.Contains(search));
        }

        if (parameters.IsActive.HasValue)
        {
            baseQuery = baseQuery.Where(p => p.IsActive == parameters.IsActive.Value);
        }

        baseQuery = baseQuery.OrderByDynamic(entitySortColumn, parameters.SortDirection);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (Enumerable.Empty<PositionGridItemDto>(), 0);
        }

        var items = await baseQuery
            .Include(p => p.Level)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ProjectToType<PositionGridItemDto>()
            .ToListAsync(cancellationToken);

        var finalItems = items.Select((item, index) => item with
        {
            RowIndex = ((parameters.PageNumber - 1) * parameters.PageSize) + index + 1
        }).ToList();

        return (finalItems, totalCount);
    }
}
