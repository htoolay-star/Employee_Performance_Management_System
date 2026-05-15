using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.TeamKPI;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

using Mapster;
namespace EPMS.Domain.Services.Performance
{
    public interface ITeamKPIService
    {
        Task<SuccessResponse<IEnumerable<TeamKPIDto>>> GetAllAsync();
        Task<SuccessResponse<TeamKPIDto>> GetByIdAsync(long id);
        Task<SuccessResponse<IEnumerable<TeamKPIDto>>> GetByTeamIdAsync(long teamId);
        Task<SuccessResponse<long>> CreateAsync(CreateTeamKPIDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateTeamKPIDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
    }

    public class TeamKPIService : ITeamKPIService
    {
        private readonly IUnitOfWork _uow;
        
        public TeamKPIService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<SuccessResponse<IEnumerable<TeamKPIDto>>> GetAllAsync()
        {
            var items = await _uow.Perf.TeamKPIs.GetAllAsync();
            var dtos = items.Adapt<IEnumerable<TeamKPIDto>>();
            return SuccessResponse<IEnumerable<TeamKPIDto>>.Ok(dtos, TeamKPIMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<TeamKPIDto>> GetByIdAsync(long id)
        {
            var item = await _uow.Perf.TeamKPIs.GetByIdAsync(id);
            if (item == null)
                return SuccessResponse<TeamKPIDto>.Fail(TeamKPIMsg.NotFound(id), ErrorType.NotFound);

            var dto = item.Adapt<TeamKPIDto>();
            return SuccessResponse<TeamKPIDto>.Ok(dto, TeamKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<TeamKPIDto>>> GetByTeamIdAsync(long teamId)
        {
            var items = await _uow.Perf.TeamKPIs.GetByTeamIdAsync(teamId);
            var dtos = items.Adapt<IEnumerable<TeamKPIDto>>();
            return SuccessResponse<IEnumerable<TeamKPIDto>>.Ok(dtos, TeamKPIMsg.RetrievedByTeam);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateTeamKPIDto dto)
        {
            if (await _uow.Perf.TeamKPIs.ExistsAsync(dto.TeamId, dto.KPIId))
                return SuccessResponse<long>.Fail(TeamKPIMsg.DuplicateEntry, ErrorType.Conflict);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse<long>.Fail(TeamKPIMsg.PriorityNotFound, ErrorType.NotFound);

            var entity = new TeamKPI(dto.TeamId, dto.KPIId, priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.TeamKPIs.Add(entity);
            await _uow.CompleteAsync();

            return SuccessResponse<long>.Ok(entity.Id, TeamKPIMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateTeamKPIDto dto)
        {
            var entity = await _uow.Perf.TeamKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(TeamKPIMsg.NotFound(id), ErrorType.NotFound);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse.Fail(TeamKPIMsg.PriorityNotFound, ErrorType.NotFound);

            entity.Update(priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.TeamKPIs.Update(entity);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(TeamKPIMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var entity = await _uow.Perf.TeamKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(TeamKPIMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.TeamKPIs.Delete(entity);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(TeamKPIMsg.Deleted);
        }
    }
}
