using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

using Mapster;
namespace EPMS.Domain.Services.Auth
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _uow;
        
        public PermissionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<SuccessResponse<IEnumerable<PermissionDto>>> GetAllPermissionsAsync()
        {
            var permissions = await _uow.Auth.Permissions.GetAllAsync();
            var dtos = permissions.Adapt<IEnumerable<PermissionDto>>();
            return SuccessResponse<IEnumerable<PermissionDto>>.Ok(dtos, PermissionMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<PermissionDto>> GetPermissionByIdAsync(long id)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null)
                return SuccessResponse<PermissionDto>.Fail(PermissionMsg.NotFound, ErrorType.NotFound);

            var dto = permission.Adapt<PermissionDto>();
            return SuccessResponse<PermissionDto>.Ok(dto, PermissionMsg.Retrieved);
        }
    }
}
