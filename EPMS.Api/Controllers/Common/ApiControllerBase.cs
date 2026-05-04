using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected ActionResult<SuccessResponse> HandleResult(SuccessResponse response)
        {
            if (response.Success) return Ok(response);

            return response.Error switch
            {
                ErrorType.NotFound => NotFound(response),
                ErrorType.Unauthorized => Unauthorized(response),
                ErrorType.Conflict => Conflict(response),
                ErrorType.Validation => BadRequest(response),
                _ => BadRequest(response)
            };
        }

        protected ActionResult<SuccessResponse<T>> HandleResult<T>(SuccessResponse<T> response)
        {
            if (response.Success)
            {
                if (response.Data == null) return NotFound(response);

                return Ok(response);
            }

            return response.Error switch
            {
                ErrorType.NotFound => NotFound(response),
                ErrorType.Unauthorized => Unauthorized(response),
                ErrorType.Conflict => Conflict(response),
                ErrorType.Validation => BadRequest(response),
                _ => BadRequest(response)
            };
        }
    }
}
