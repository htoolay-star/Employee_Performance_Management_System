using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.App;

public interface IExcelService
{
    Task<SuccessResponse<byte[]>> ExportAsync<T>(IEnumerable<T> data, string sheetName = "Sheet1");
    Task<SuccessResponse<IEnumerable<T>>> ImportAsync<T>(Stream stream) where T : class, new();
    Task<SuccessResponse<IEnumerable<T>>> ImportAsync<T>(byte[] bytes) where T : class, new();
}
