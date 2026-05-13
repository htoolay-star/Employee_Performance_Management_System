using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Extensions;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Features.EmployeeProfiles;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeeProfileRepository : GenericRepository<EmployeeProfile>, IEmployeeProfileRepository
    {
        public EmployeeProfileRepository(AppDbContext context) : base(context) { }

        public async Task<EmployeeProfile?> GetByPublicIdAsync(Guid publicId) =>
            await _dbSet.FirstOrDefaultAsync(p => p.PublicId == publicId);

        public async Task<EmployeeProfile?> GetByStaffNoAsync(string staffNo) =>
            await _dbSet.FirstOrDefaultAsync(p => p.StaffNo == staffNo);

        public async Task<EmployeeProfile?> GetByUserIdAsync(long userId) =>
            await _dbSet.FirstOrDefaultAsync(p => p.UserId == userId);

        public async Task<bool> ExistsByEmailAsync(string email, long? excludeId = null)
        {
            var query = _dbSet.Where(p => p.EmailAddress == email);
            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<EmployeeLookupDto>> GetLookupDtoAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Select(p => new EmployeeLookupDto
                {
                    Id = p.Id,
                    StaffNo = p.StaffNo,
                    StaffName = p.StaffName,
                    PositionId = p.Employment != null ? p.Employment.PositionId : null,
                })
                .ToListAsync();
        }

        public async Task<(IEnumerable<EmployeeProfileGridItemDto> Items, int TotalCount)> GetPagedDtoAsync(
            EmployeeProfileQueryParameters parameters, string entitySortColumn, CancellationToken cancellationToken = default)
        {
            IQueryable<EmployeeProfile> query = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.Trim();

                query = query.Where(p =>
                    p.StaffNo.Contains(searchTerm) ||
                    p.StaffName.Contains(searchTerm) ||
                    (p.EmailAddress != null && p.EmailAddress.Contains(searchTerm)));
            }

            if (parameters.DepartmentId.HasValue)
            {
                query = query.Where(p => p.Employment != null && p.Employment.DepartmentId == parameters.DepartmentId.Value);
            }

            if (parameters.TeamId.HasValue)
            {
                query = query.Where(p => p.Employment != null && p.Employment.TeamId == parameters.TeamId.Value);
            }

            if (parameters.PositionId.HasValue)
            {
                query = query.Where(p => p.Employment != null && p.Employment.PositionId == parameters.PositionId.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.EmploymentStatus))
            {
                query = query.Where(p => p.Employment != null && p.Employment.EmploymentStatus == parameters.EmploymentStatus);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            if (totalCount == 0)
            {
                return (Enumerable.Empty<EmployeeProfileGridItemDto>(), 0);
            }

            query = query.OrderByDynamic(entitySortColumn, parameters.SortDirection);

            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ProjectToType<EmployeeProfileGridItemDto>()
                .ToListAsync(cancellationToken);

            var finalItems = items.Select((item, index) => item with
            {
                RowIndex = ((parameters.PageNumber - 1) * parameters.PageSize) + index + 1
            }).ToList();

            return (finalItems, totalCount);
        }
    }
}
