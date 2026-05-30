using EPMS.Api.Controllers.Common;
using EPMS.Api.Jobs;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Hangfire;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EPMS.Api.Controllers.Info;

[Route("api/employee-profiles")]
[ApiController]
public class EmployeeProfilesController : ApiControllerBase
{
    private static readonly string[] _allowedExtensions = { ".xlsx", ".xlsm", ".csv" };

    private static bool IsValidExcelFile(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        return ext != null && _allowedExtensions.Contains(ext);
    }

    private readonly IEmployeeProfileService _profileService;
    private readonly IExcelService _excelService;

    public EmployeeProfilesController(IEmployeeProfileService profileService, IExcelService excelService)
    {
        _profileService = profileService;
        _excelService = excelService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeProfileDto>>>> GetAll()
    {
        var result = await _profileService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeLookupDto>>>> GetLookup()
    {
        var result = await _profileService.GetLookupAsync();
        return HandleResult(result);
    }

    [HttpGet("direct-reports/lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeLookupDto>>>> GetDirectReportsLookup()
    {
        var result = await _profileService.GetDirectReportsLookupAsync();
        return HandleResult(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<SuccessResponse<PaginatedResponse<EmployeeProfileGridItemDto>>>> GetPaged([FromQuery] EPMS.Shared.Features.EmployeeProfiles.EmployeeProfileQueryParameters parameters)
    {
        var result = await _profileService.GetPagedAsync(parameters);
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeProfileDto>>> GetById(long id)
    {
        var result = await _profileService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-publicid/{publicId:guid}")]
    public async Task<ActionResult<SuccessResponse<EmployeeProfileDto>>> GetByPublicId(Guid publicId)
    {
        var result = await _profileService.GetByPublicIdAsync(publicId);
        return HandleResult(result);
    }

    [HttpGet("by-staffno/{staffNo}")]
    public async Task<ActionResult<SuccessResponse<EmployeeProfileDto>>> GetByStaffNo(string staffNo)
    {
        var result = await _profileService.GetByStaffNoAsync(staffNo);
        return HandleResult(result);
    }

    [HttpGet("by-user/{userId:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeProfileDto>>> GetByUserId(long userId)
    {
        var result = await _profileService.GetByUserIdAsync(userId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateEmployeeProfileDto dto)
    {
        var result = await _profileService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPost("full-create")]
    public async Task<ActionResult<SuccessResponse<long>>> CreateFull(CreateFullEmployeeDto dto)
    {
        var result = await _profileService.CreateFullAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateEmployeeProfileDto dto)
    {
        var result = await _profileService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _profileService.DeleteAsync(id);
        return HandleResult(result);
    }

    [HttpGet("export")]
    public async Task<ActionResult> Export()
    {
        var result = await _profileService.GetFullExportAsync();
        if (!result.Success || result.Data == null)
            return BadRequest(result);

        var excelResult = await _excelService.ExportAsync(result.Data, "Employees");
        if (!excelResult.Success)
            return BadRequest(excelResult);

        return File(excelResult.Data,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"employees-{DateTime.Now:yyyy-MM-dd}.xlsx");
    }

    [HttpGet("export-template")]
    public async Task<ActionResult> ExportTemplate()
    {
        var emptyList = new List<EmployeeFullImportRow> { new() };
        var excelResult = await _excelService.ExportAsync(emptyList, "Template");
        if (!excelResult.Success)
            return BadRequest(excelResult);

        return File(excelResult.Data,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "employee-import-template.xlsx");
    }

    [HttpPost("import-preview")]
    public async Task<ActionResult<SuccessResponse<ImportPreviewResult>>> ImportPreview(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(SuccessResponse<ImportPreviewResult>.Fail("No file uploaded.", ErrorType.Validation));

        if (!IsValidExcelFile(file))
            return BadRequest(SuccessResponse<ImportPreviewResult>.Fail("Only .xlsx, .xlsm, and .csv files are allowed.", ErrorType.Validation));

        using var stream = file.OpenReadStream();
        var importResult = await _excelService.ImportAsync<EmployeeFullImportRow>(stream);
        if (!importResult.Success || importResult.Data == null)
            return BadRequest(importResult);

        var result = await _profileService.ImportPreviewAsync(importResult.Data.ToList());
        return HandleResult(result);
    }

    [HttpPost("import")]
    public async Task<ActionResult<SuccessResponse<string>>> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(SuccessResponse<string>.Fail("No file uploaded.", ErrorType.Validation));

        if (!IsValidExcelFile(file))
            return BadRequest(SuccessResponse<string>.Fail("Only .xlsx, .xlsm, and .csv files are allowed.", ErrorType.Validation));

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var jobId = BackgroundJob.Enqueue<EmployeeImportJob>(
            job => job.ImportAsync(bytes, file.FileName, null!));

        return Ok(SuccessResponse<string>.Ok(jobId, "Import queued."));
    }

    [HttpGet("import-status/{jobId}")]
    public async Task<ActionResult<SuccessResponse<ImportResult>>> GetImportStatus(string jobId)
    {
        var cache = HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
        var cached = await cache.GetAsync($"import:{jobId}");

        if (cached == null)
        {
            var monitor = HttpContext.RequestServices.GetRequiredService<Hangfire.Storage.IMonitoringApi>();
            var job = monitor.JobDetails(jobId);
            if (job == null)
                return Ok(SuccessResponse<ImportResult>.Fail("Import job not found.", ErrorType.NotFound));
            return Ok(SuccessResponse<ImportResult>.Ok(null, "Import is processing..."));
        }

        var result = JsonSerializer.Deserialize<ImportResult>(cached);
        await cache.RemoveAsync($"import:{jobId}");
        return Ok(SuccessResponse<ImportResult>.Ok(result, "Import completed."));
    }
}
