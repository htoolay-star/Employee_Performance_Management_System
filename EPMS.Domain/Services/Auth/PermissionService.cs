using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;

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
            return SuccessResponse<IEnumerable<PermissionDto>>.Ok(dtos, "Permissions retrieved successfully.");
        }

        public async Task<SuccessResponse<PermissionDto>> GetPermissionByIdAsync(int id)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null)
                return SuccessResponse<PermissionDto>.Fail("Permission not found.", ErrorType.NotFound);

            var dto = _mapper.Map<PermissionDto>(permission);
            return SuccessResponse<PermissionDto>.Ok(dto, "Permission retrieved successfully.");
        }

        public async Task<SuccessResponse> CreatePermissionAsync(CreatePermissionDto dto)
        {
            if (!await _uow.Auth.Permissions.IsCodeUniqueAsync(dto.Code))
                return SuccessResponse.Fail("Permission code already exists.", ErrorType.Conflict);

            var permission = new Permission(dto.Code, dto.Name, dto.Description);

            _uow.Auth.Permissions.Add(permission);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok("Permission created successfully.");
        }

        public async Task<SuccessResponse> UpdatePermissionAsync(int id, UpdatePermissionDto dto)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null)
                return SuccessResponse.Fail("Permission not found.", ErrorType.NotFound);

            permission.UpdateDetails(dto.Name, dto.Description);

            await _uow.CompleteAsync();
            return SuccessResponse.Ok("Permission updated successfully.");
        }

        public async Task<SuccessResponse> DeletePermissionAsync(int id)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null)
                return SuccessResponse.Fail("Permission not found.", ErrorType.NotFound);

            _uow.Auth.Permissions.Delete(permission);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok("Permission deleted successfully.");
        }
    }
}
