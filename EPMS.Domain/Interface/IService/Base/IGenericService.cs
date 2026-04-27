using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Base
{
    public interface IGenericService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : class
    {
        Task<IEnumerable<TDto>> GetAllAsync(CancellationToken ct = default);
        Task<TDto?> GetByIdAsync(object id, CancellationToken ct = default);
        Task CreateAsync(TCreateDto dto, CancellationToken ct = default);
        Task UpdateAsync(object id, TUpdateDto dto, CancellationToken ct = default);
        Task DeleteAsync(object id, CancellationToken ct = default);
    }
}