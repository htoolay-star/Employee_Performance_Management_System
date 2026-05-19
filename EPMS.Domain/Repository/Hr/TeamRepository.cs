using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Extensions;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Repository.Base;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Features.Teams;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Hr;

public class TeamRepository : GenericRepository<Team>, ITeamRepository
{
    public TeamRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Team>> GetTeamsByDepartmentAsync(long departmentId)
    {
        return await _dbSet
            .Where(t => t.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameInDepartmentAsync(string name, long departmentId, long? excludeId = null)
    {
        var query = _dbSet.Where(t => t.DepartmentId == departmentId && t.Name == name);
        
        if (excludeId.HasValue)
            query = query.Where(t => t.Id != excludeId.Value);
        
        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByCodeAsync(string code, long? excludeId = null)
    {
        var normalized = code.Trim().ToUpperInvariant();
        var query = _dbSet.Where(t => t.Code == normalized);
        
        if (excludeId.HasValue)
            query = query.Where(t => t.Id != excludeId.Value);
        
        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByIdAsync(long id)
    {
        return await _dbSet.AnyAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<LookUpDto>> GetLookupDtoAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Select(p => new LookUpDto
            {
                Id = p.Id,
                Name = p.Name,
                IsActive = p.IsActive,
            })
            .ToListAsync();
    }

    public async Task<(IEnumerable<TeamGridItemDto> Items, int TotalCount)> GetPagedAsync(TeamQueryParameters parameters, string entitySortColumn, CancellationToken cancellationToken = default)
    {
        IQueryable<Team> baseQuery = _dbSet.AsNoTracking().Include(t => t.Department);

        if (parameters.DepartmentId.HasValue)
        {
            baseQuery = baseQuery.Where(t => t.DepartmentId == parameters.DepartmentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var search = parameters.SearchTerm.Trim();
            baseQuery = baseQuery.Where(t => t.Name.Contains(search));
        }

        // Add IsActive filter for actual data retrieval
        if (parameters.IsActive.HasValue)
        {
            baseQuery = baseQuery.Where(t => t.IsActive == parameters.IsActive.Value);
        }

        baseQuery = baseQuery.OrderByDynamic(entitySortColumn, parameters.SortDirection);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (Enumerable.Empty<TeamGridItemDto>(), 0);
        }

        var items = await baseQuery
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ProjectToType<TeamGridItemDto>()
            .ToListAsync(cancellationToken);

        var finalItems = items.Select((item, index) => item with
        {
            RowIndex = ((parameters.PageNumber - 1) * parameters.PageSize) + index + 1
        }).ToList();

        return (finalItems, totalCount);
    }
}
