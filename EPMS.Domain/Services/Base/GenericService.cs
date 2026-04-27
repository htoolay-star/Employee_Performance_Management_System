using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Interface.IService.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Services.Base
{
    public class GenericService<TEntity, TDto, TCreateDto, TUpdateDto>
        : IGenericService<TEntity, TDto, TCreateDto, TUpdateDto> where TEntity : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ၁။ GetAllAsync: Table ထဲက data တွေအကုန်ထုတ်ပြီး DTO ပြောင်းပေးတာ
        public virtual async Task<IEnumerable<TDto>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _repository.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        // ၂။ GetByIdAsync: ID နဲ့ ရှာပြီး DTO အနေနဲ့ ပြန်ပေးတာ
        public virtual async Task<TDto?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity == null ? default : _mapper.Map<TDto>(entity);
        }

        // ၃။ CreateAsync: User ပေးလိုက်တဲ့ DTO ကို Entity ပြောင်းပြီး DB ထဲ ထည့်တာ
        public virtual async Task CreateAsync(TCreateDto dto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        // ၄။ UpdateAsync: ရှိပြီးသား data ကိုရှာပြီး DTO အသစ်နဲ့ အစားထိုးတာ
        public virtual async Task UpdateAsync(object id, TUpdateDto dto, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            if (entity == null) throw new Exception("ရှာမတွေ့ပါ");

            _mapper.Map(dto, entity); // Update DTO data to existing entity
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        // ၅။ DeleteAsync: Data ကို ဖျက်ထုတ်တာ
        public virtual async Task DeleteAsync(object id, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            if (entity != null)
            {
                _repository.Delete(entity);
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }
    }
}
