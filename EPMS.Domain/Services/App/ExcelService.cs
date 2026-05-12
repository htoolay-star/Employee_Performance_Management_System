using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using MiniExcelLibs;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.App;

public class ExcelService : IExcelService
{
    public async Task<SuccessResponse<byte[]>> ExportAsync<T>(IEnumerable<T> data, string sheetName = "Sheet1")
    {
        try
        {
            using var stream = new MemoryStream();
            await stream.SaveAsAsync(data, sheetName: sheetName);
            return SuccessResponse<byte[]>.Ok(stream.ToArray(), ExcelMsg.Exported);
        }
        catch (Exception ex)
        {
            return SuccessResponse<byte[]>.Fail(ExcelMsg.ExportFailed(ex.Message), ErrorType.ServerError);
        }
    }

    public async Task<SuccessResponse<IEnumerable<T>>> ImportAsync<T>(Stream stream) where T : class, new()
    {
        try
        {
            var data = (await stream.QueryAsync<T>()).ToList();

            return SuccessResponse<IEnumerable<T>>.Ok(data, ExcelMsg.Imported);
        }
        catch (Exception ex)
        {
            return SuccessResponse<IEnumerable<T>>.Fail(ExcelMsg.ImportFailed(ex.Message), ErrorType.ServerError);
        }
    }

    public async Task<SuccessResponse<IEnumerable<T>>> ImportAsync<T>(byte[] bytes) where T : class, new()
    {
        using var stream = new MemoryStream(bytes);
        return await ImportAsync<T>(stream);
    }
}