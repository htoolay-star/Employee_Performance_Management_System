using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _uow.Auth.Permissions.GetAllAsync();
            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }

        public async Task<PermissionDto?> GetPermissionByIdAsync(int id)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null) throw new Exception("Permission not found");

            return _mapper.Map<PermissionDto>(permission);
        }

 
        public async Task CreatePermissionAsync(CreatePermissionDto dto)
        {
            
            if (!await _uow.Auth.Permissions.IsCodeUniqueAsync(dto.Code))
            {
                throw new Exception("Permission Code already have");
            }
            
            var permission = new Permission(dto.Code, dto.Name, dto.Description);

            _uow.Auth.Permissions.Add(permission);
            await _uow.CompleteAsync();
        }

        
        public async Task UpdatePermissionAsync(int id, UpdatePermissionDto dto)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null) throw new Exception("Permission not found");

            permission.UpdateDetails(dto.Name, dto.Description);

            await _uow.CompleteAsync();
        }

        public async Task DeletePermissionAsync(int id)
        {
            var permission = await _uow.Auth.Permissions.GetByIdAsync(id);

            if (permission == null) throw new Exception("Permission not found");

            if (permission != null)
            {
                _uow.Auth.Permissions.Delete(permission);
                await _uow.CompleteAsync();
            }
        }
    }
}
