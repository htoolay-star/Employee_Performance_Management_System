using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Auth
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PermissionService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<IEnumerable<PermissionDto>>> GetAllPermissionsAsync()
        {
            var permissions = await _uow.Auth.Permissions.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);
            return SuccessResponse<IEnumerable<PermissionDto>>.Ok(dtos, PermissionMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<PermissionDto>> GetPermissionByIdAsync(long id)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null)
                return SuccessResponse<PermissionDto>.Fail(PermissionMsg.NotFound, ErrorType.NotFound);

            var dto = _mapper.Map<PermissionDto>(permission);
            return SuccessResponse<PermissionDto>.Ok(dto, PermissionMsg.Retrieved);
        }

        public async Task<SuccessResponse<long>> CreatePermissionAsync(CreatePermissionDto dto)
        {
            if (!await _uow.Auth.Permissions.IsCodeUniqueAsync(dto.Code))
                return SuccessResponse<long>.Fail("Permission code already exists.", ErrorType.Conflict);

            var permission = new Permission(dto.Code, dto.Name, dto.Description);

            _uow.Auth.Permissions.Add(permission);
            await _uow.CompleteAsync();
            return SuccessResponse<long>.Ok(permission.Id, PermissionMsg.Created);
        }

        public async Task<SuccessResponse> UpdatePermissionAsync(long id, UpdatePermissionDto dto)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null)
                return SuccessResponse.Fail(PermissionMsg.NotFound, ErrorType.NotFound);

            permission.UpdateDetails(dto.Name, dto.Description);

            await _uow.CompleteAsync();
            return SuccessResponse.Ok(PermissionMsg.Updated);
        }

        public async Task<SuccessResponse> DeletePermissionAsync(long id)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null)
                return SuccessResponse.Fail(PermissionMsg.NotFound, ErrorType.NotFound);

            _uow.Auth.Permissions.Delete(permission);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok(PermissionMsg.Deleted);
        }
    }
}
