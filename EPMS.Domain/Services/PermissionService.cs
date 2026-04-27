using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _permissionRepo = permissionRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }

        public async Task<PermissionDto?> GetPermissionByIdAsync(int id)
        {
            var permission = await _permissionRepo.GetByIdAsync(id);

            if (permission == null) throw new Exception("Permission not found");

            return _mapper.Map<PermissionDto>(permission);
        }

 
        public async Task CreatePermissionAsync(CreatePermissionDto dto)
        {
            
            if (!await _permissionRepo.IsCodeUniqueAsync(dto.Code))
            {
                throw new Exception("Permission Code already have");
            }
            
            var permission = new Permission(dto.Code, dto.Name, dto.Description);

            _permissionRepo.Add(permission);
            await _unitOfWork.CompleteAsync();
        }

        
        public async Task UpdatePermissionAsync(int id, UpdatePermissionDto dto)
        {
            var permission = await _permissionRepo.GetByIdAsync(id);

            if (permission == null) throw new Exception("Permission not found");

            permission.UpdateDetails(dto.Name, dto.Description);

            _permissionRepo.Update(permission);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePermissionAsync(int id)
        {
            var permission = await _permissionRepo.GetByIdAsync(id);

            if (permission == null) throw new Exception("Permission not found");

            if (permission != null)
            {
                
                permission.Deactivate();
                _permissionRepo.Update(permission);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
