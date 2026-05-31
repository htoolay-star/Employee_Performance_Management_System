using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.MeetingStatuses;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Performance
{
    public class OneOnOneMeetingService : IOneOnOneMeetingService
    {
        private readonly IUnitOfWork _uow;
        private readonly TimeProvider _timeProvider;
        private readonly ICurrentEmployeeContextService _currentEmployeeContext;

        public OneOnOneMeetingService(IUnitOfWork uow, TimeProvider timeProvider, ICurrentEmployeeContextService currentEmployeeContext)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _currentEmployeeContext = currentEmployeeContext;
        }

        private async Task<bool> IsManagerOrAdminAsync(long managerId)
        {
            var employeeId = await _currentEmployeeContext.GetEmployeeIdAsync();
            return _currentEmployeeContext.IsAdmin || employeeId == managerId;
        }

        private async Task<bool> IsOwnerAsync(long employeeId)
        {
            var currentEmployeeId = await _currentEmployeeContext.GetEmployeeIdAsync();
            return currentEmployeeId == employeeId;
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetAllAsync()
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetAllAsync();
            var dtos = meetings.Adapt<IEnumerable<OneOnOneMeetingDto>>();
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetUpcomingAsync()
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetUpcomingAsync();
            var dtos = meetings.Adapt<IEnumerable<OneOnOneMeetingDto>>();
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedUpcoming);
        }

        public async Task<SuccessResponse<OneOnOneMeetingDto>> GetByIdAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse<OneOnOneMeetingDto>.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            var dto = meeting.Adapt<OneOnOneMeetingDto>();
            return SuccessResponse<OneOnOneMeetingDto>.Ok(dto, OneOnOneMeetingMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByEmployeeIdAsync(long employeeId)
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetByEmployeeIdAsync(employeeId);
            var dtos = meetings.Adapt<IEnumerable<OneOnOneMeetingDto>>();
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByManagerIdAsync(long managerId)
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetByManagerIdAsync(managerId);
            var dtos = meetings.Adapt<IEnumerable<OneOnOneMeetingDto>>();
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateOneOnOneMeetingDto dto)
        {
            var meeting = new OneOnOneMeeting(
                dto.EmployeeId,
                dto.ManagerId,
                dto.Title,
                dto.ScheduledDate,
                dto.ScheduledEndTime);

            if (dto.RelatedPIPId.HasValue)
            {
                meeting.LinkToPIP(dto.RelatedPIPId.Value);
            }

            _uow.Perf.OneOnOneMeetings.Add(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse<long>.Ok(meeting.Id, OneOnOneMeetingMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateOneOnOneMeetingDto dto)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (meeting.Status == Completed || meeting.Status == Cancelled)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.AlreadyCompleted, ErrorType.Validation);

            if (!await IsManagerOrAdminAsync(meeting.ManagerId) && !await IsOwnerAsync(meeting.EmployeeId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            meeting.Update(dto.Title, dto.ScheduledDate, dto.ScheduledEndTime);

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (!await IsManagerOrAdminAsync(meeting.ManagerId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            _uow.Perf.OneOnOneMeetings.Delete(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Deleted);
        }

        public async Task<SuccessResponse> CompleteAsync(long id, CompleteMeetingDto dto)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (!await IsManagerOrAdminAsync(meeting.ManagerId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            meeting.CompleteMeeting(dto.Summary, dto.DiscussionNotes, dto.PrivateNotes, dto.ActionItems, _timeProvider);

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Completed);
        }

        public async Task<SuccessResponse> CancelAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (!await IsManagerOrAdminAsync(meeting.ManagerId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            meeting.Cancel();

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Cancelled);
        }

        public async Task<SuccessResponse> AcknowledgeAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (!await IsOwnerAsync(meeting.EmployeeId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            meeting.AcknowledgeByEmployee(_timeProvider);

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Acknowledged);
        }
    }
}
