using EPMS.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers
{
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;

        public ApiBaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
