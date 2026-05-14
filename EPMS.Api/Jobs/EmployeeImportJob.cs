using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Hangfire;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EPMS.Api.Jobs;

public class EmployeeImportJob
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    public EmployeeImportJob(IServiceScopeFactory scopeFactory, IDistributedCache cache)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task ImportAsync(byte[] fileBytes, string fileName, string jobId)
    {
        ImportResult result;
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var excelService = scope.ServiceProvider.GetRequiredService<IExcelService>();
            var profileService = scope.ServiceProvider.GetRequiredService<IEmployeeProfileService>();

            using var stream = new MemoryStream(fileBytes);
            var importResult = await excelService.ImportAsync<EmployeeFullImportRow>(stream);
            if (!importResult.Success || importResult.Data == null)
            {
                result = new ImportResult
                {
                    TotalRows = 0,
                    SuccessCount = 0,
                    ErrorCount = 1,
                    Errors = [importResult?.Message ?? "Failed to parse Excel file."]
                };
            }
            else
            {
                var serviceResult = await profileService.ImportFullEmployeesAsync(importResult.Data.ToList());
                result = serviceResult.Data ?? new ImportResult
                {
                    TotalRows = importResult.Data.Count(),
                    SuccessCount = 0,
                    ErrorCount = 1,
                    Errors = [serviceResult.Message]
                };
            }
        }
        catch (Exception ex)
        {
            result = new ImportResult
            {
                TotalRows = 0,
                SuccessCount = 0,
                ErrorCount = 1,
                Errors = [$"Import failed: {ex.Message}"]
            };
        }

        var serialized = JsonSerializer.SerializeToUtf8Bytes(result);
        await _cache.SetAsync($"import:{jobId}", serialized, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });
    }
}
