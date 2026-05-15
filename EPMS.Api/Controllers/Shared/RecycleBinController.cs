using EPMS.Api.Controllers.Common;
using EPMS.Domain.Services.Shared;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.RecycleBin;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Shared
{
    [Route("api/recycle-bin")]
    [ApiController]
    public class RecycleBinController : ApiControllerBase
    {
        private readonly IRecycleBinService _recycleBinService;

        public RecycleBinController(IRecycleBinService recycleBinService)
        {
            _recycleBinService = recycleBinService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<RecycleBinItemDto>>>> GetAll()
        {
            var result = await _recycleBinService.GetAllDeletedAsync();
            return HandleResult(result);
        }

        [HttpPost("restore/{entityType}/{entityId:long}")]
        public async Task<ActionResult<SuccessResponse>> Restore(string entityType, long entityId)
        {
            var result = await _recycleBinService.RestoreAsync(entityType, entityId);
            return HandleResult(result);
        }
    }
}
