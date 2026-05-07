using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;
using static EPMS.Shared.Constants.MeetingStatuses;

namespace EPMS.Domain.Services.Performance
{
    public class OneOnOneMeetingService : IOneOnOneMeetingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly TimeProvider _timeProvider;

        public OneOnOneMeetingService(IUnitOfWork uow, IMapper mapper, TimeProvider timeProvider)
        {
            _uow = uow;
            _mapper = mapper;
            _timeProvider = timeProvider;
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetAllAsync()
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<OneOnOneMeetingDto>>(meetings);
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetUpcomingAsync()
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetUpcomingAsync();
            var dtos = _mapper.Map<IEnumerable<OneOnOneMeetingDto>>(meetings);
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedUpcoming);
        }

        public async Task<SuccessResponse<OneOnOneMeetingDto>> GetByIdAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse<OneOnOneMeetingDto>.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            var dto = _mapper.Map<OneOnOneMeetingDto>(meeting);
            return SuccessResponse<OneOnOneMeetingDto>.Ok(dto, OneOnOneMeetingMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByEmployeeIdAsync(long employeeId)
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetByEmployeeIdAsync(employeeId);
            var dtos = _mapper.Map<IEnumerable<OneOnOneMeetingDto>>(meetings);
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByManagerIdAsync(long managerId)
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetByManagerIdAsync(managerId);
            var dtos = _mapper.Map<IEnumerable<OneOnOneMeetingDto>>(meetings);
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateOneOnOneMeetingDto dto)
        {
            var meeting = new OneOnOneMeeting(
                dto.EmployeeId,
                dto.ManagerId,
                dto.Title,
                dto.ScheduledDate);

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

            meeting.GetType().GetProperty("Title")?.SetValue(meeting, dto.Title.Trim());
            meeting.GetType().GetProperty("ScheduledDate")?.SetValue(meeting, dto.ScheduledDate);

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.OneOnOneMeetings.Delete(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Deleted);
        }

        public async Task<SuccessResponse> CompleteAsync(long id, CompleteMeetingDto dto)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

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

            meeting.AcknowledgeByEmployee(_timeProvider);

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Acknowledged);
        }
    }
}