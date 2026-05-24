using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Features.EmployeeProfiles;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IEmployeeProfileRepository : IGenericRepository<EmployeeProfile>
    {
        Task<EmployeeProfile?> GetByPublicIdAsync(Guid publicId);
        Task<EmployeeProfile?> GetByStaffNoAsync(string code);
        Task<EmployeeProfile?> GetByUserIdAsync(long userId);
        Task<bool> ExistsByEmailAsync(string email, long? excludeId = null);
        Task<IEnumerable<EmployeeLookupDto>> GetLookupDtoAsync();
        Task<(IEnumerable<EmployeeProfileGridItemDto> Items, int TotalCount)> GetPagedDtoAsync(
            EmployeeProfileQueryParameters parameters, string entitySortColumn, HashSet<long>? excludeEmployeeIds = null, CancellationToken cancellationToken = default);
    }
}
