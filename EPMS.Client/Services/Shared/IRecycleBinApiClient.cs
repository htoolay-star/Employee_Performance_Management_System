using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.RecycleBin;
using Refit;

namespace EPMS.Client.Services.Shared;

public interface IRecycleBinApiClient
{
    [Get("/api/recycle-bin")]
    Task<SuccessResponse<IEnumerable<RecycleBinItemDto>>> GetAllAsync();

    [Post("/api/recycle-bin/restore/{entityType}/{entityId}")]
    Task<SuccessResponse> RestoreAsync(string entityType, long entityId);
}
