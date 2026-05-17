using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

using Mapster;
namespace EPMS.Domain.Services.Performance
{
    public class KPIMasterService : IKPIMasterService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        
        public KPIMasterService(IUnitOfWork uow, ICacheService cacheService)
        {
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync()
        {
            var dtos = await _cacheService.GetOrCreateAsync(
                CacheKeys.Performance.KPIMasterLookups(),
                async () => await _uow.Perf.KPIMasters.GetLookupDtoAsync(),
                TimeSpan.FromHours(12)
            );

            return SuccessResponse<IEnumerable<LookUpDto>>.Ok(dtos ?? [], KPIMasterMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<KPIMasterDto>>> GetAllAsync()
        {
            var kpis = await _uow.Perf.KPIMasters.GetAllAsync();
            var dtos = kpis.Adapt<IEnumerable<KPIMasterDto>>();
            return SuccessResponse<IEnumerable<KPIMasterDto>>.Ok(dtos, KPIMasterMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<KPIMasterDto>>> GetActiveAsync()
        {
            var kpis = await _uow.Perf.KPIMasters.GetActiveAsync();
            var dtos = kpis.Adapt<IEnumerable<KPIMasterDto>>();
            return SuccessResponse<IEnumerable<KPIMasterDto>>.Ok(dtos, KPIMasterMsg.RetrievedActive);
        }

        public async Task<SuccessResponse<KPIMasterDto>> GetByIdAsync(long id)
        {
            var kpi = await _uow.Perf.KPIMasters.GetByIdAsync(id);

            if (kpi == null)
                return SuccessResponse<KPIMasterDto>.Fail(KPIMasterMsg.NotFound(id), ErrorType.NotFound);

            var dto = kpi.Adapt<KPIMasterDto>();
            return SuccessResponse<KPIMasterDto>.Ok(dto, KPIMasterMsg.Retrieved);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateKPIMasterDto dto)
        {
            if (await _uow.Perf.KPIMasters.CodeExistsAsync(dto.Code))
            {
                return SuccessResponse<long>.Fail(string.Format(KPIMasterMsg.DuplicateCode, dto.Code), ErrorType.Conflict);
            }

            var kpi = new KPIMaster(dto.CategoryId, dto.Code, dto.Name, dto.Description, dto.ScoringDirection);

            _uow.Perf.KPIMasters.Add(kpi);
            await _uow.CompleteAsync();
            await _cacheService.RemoveAsync(CacheKeys.Performance.KPIMasterLookups());

            return SuccessResponse<long>.Ok(kpi.Id, KPIMasterMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateKPIMasterDto dto)
        {
            var kpi = await _uow.Perf.KPIMasters.GetByIdAsync(id);

            if (kpi == null)
                return SuccessResponse.Fail(KPIMasterMsg.NotFound(id), ErrorType.NotFound);

            if (await _uow.Perf.KPIMasters.CodeExistsAsync(dto.Code, id))
            {
                return SuccessResponse.Fail(string.Format(KPIMasterMsg.DuplicateCode, dto.Code), ErrorType.Conflict);
            }

            kpi.Update(dto.CategoryId, dto.Code, dto.Name, dto.Description, dto.ScoringDirection);

            _uow.Perf.KPIMasters.Update(kpi);
            await _uow.CompleteAsync();
            await _cacheService.RemoveAsync(CacheKeys.Performance.KPIMasterLookups());

            return SuccessResponse.Ok(KPIMasterMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var kpi = await _uow.Perf.KPIMasters.GetByIdAsync(id);

            if (kpi == null)
                return SuccessResponse.Fail(KPIMasterMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.KPIMasters.Delete(kpi);
            await _uow.CompleteAsync();
            await _cacheService.RemoveAsync(CacheKeys.Performance.KPIMasterLookups());

            return SuccessResponse.Ok(KPIMasterMsg.Deleted);
        }

        public async Task<SuccessResponse> DeactivateAsync(long id)
        {
            var kpi = await _uow.Perf.KPIMasters.GetByIdAsync(id);

            if (kpi == null)
                return SuccessResponse.Fail(KPIMasterMsg.NotFound(id), ErrorType.NotFound);

            kpi.Deactivate();

            _uow.Perf.KPIMasters.Update(kpi);
            await _uow.CompleteAsync();
            await _cacheService.RemoveAsync(CacheKeys.Performance.KPIMasterLookups());

            return SuccessResponse.Ok(KPIMasterMsg.Deactivated);
        }

        public async Task<SuccessResponse> ReactivateAsync(long id)
        {
            var kpi = await _uow.Perf.KPIMasters.GetByIdAsync(id);

            if (kpi == null)
                return SuccessResponse.Fail(KPIMasterMsg.NotFound(id), ErrorType.NotFound);

            kpi.Reactivate();

            _uow.Perf.KPIMasters.Update(kpi);
            await _uow.CompleteAsync();
            await _cacheService.RemoveAsync(CacheKeys.Performance.KPIMasterLookups());

            return SuccessResponse.Ok(KPIMasterMsg.Reactivated);
        }
    }
}
