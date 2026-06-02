using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.AppDTOs;
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
        private readonly INotificationService _notificationService;

        public OneOnOneMeetingService(IUnitOfWork uow, TimeProvider timeProvider, ICurrentEmployeeContextService currentEmployeeContext, INotificationService notificationService)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _currentEmployeeContext = currentEmployeeContext;
            _notificationService = notificationService;
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

        private void SanitizePrivateNotes(IEnumerable<OneOnOneMeetingDto> dtos, long? currentEmployeeId)
        {
            if (!_currentEmployeeContext.IsAdmin && currentEmployeeId.HasValue)
            {
                foreach (var dto in dtos)
                {
                    if (dto.ManagerId != currentEmployeeId.Value)
                        dto.PrivateNotes = null;
                }
            }
        }

        private void SanitizePrivateNotes(OneOnOneMeetingDto dto, long? currentEmployeeId)
        {
            if (!_currentEmployeeContext.IsAdmin && currentEmployeeId.HasValue && dto.ManagerId != currentEmployeeId.Value)
                dto.PrivateNotes = null;
        }

        private async Task SendMeetingNotification(long targetEmployeeId, string title, string message, string type, string? redirectUrl = null)
        {
            var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(targetEmployeeId);
            if (profile?.UserId == null)
                return;

            try
            {
                await _notificationService.CreateAsync(new CreateNotificationDto
                {
                    ToUserId = profile.UserId.Value,
                    Title = title,
                    Message = message,
                    Type = type,
                    RedirectUrl = redirectUrl
                });
            }
            catch
            {
            }
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetAllAsync()
        {
            var employeeId = await _currentEmployeeContext.GetEmployeeIdAsync();

            if (_currentEmployeeContext.IsAdmin || !employeeId.HasValue)
            {
                var allMeetings = await _uow.Perf.OneOnOneMeetings.GetAllAsync();
                var allDtos = allMeetings.Adapt<IEnumerable<OneOnOneMeetingDto>>();
                SanitizePrivateNotes(allDtos, employeeId);
                return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(allDtos, OneOnOneMeetingMsg.RetrievedAll);
            }

            var directReports = await _uow.Info.EmployeeEmployments
                .FindAllAsync(e => e.DirectManagerId == employeeId.Value && !e.IsDeleted);
            var directReportIds = directReports.Select(e => e.EmployeeId).ToHashSet();

            var meetings = await _uow.Perf.OneOnOneMeetings.GetAllAsync();
            var filtered = meetings.Where(m =>
                m.EmployeeId == employeeId.Value ||
                m.ManagerId == employeeId.Value ||
                directReportIds.Contains(m.EmployeeId));
            var dtos = filtered.Adapt<IEnumerable<OneOnOneMeetingDto>>();
            SanitizePrivateNotes(dtos, employeeId);
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetUpcomingAsync()
        {
            var employeeId = await _currentEmployeeContext.GetEmployeeIdAsync();

            if (_currentEmployeeContext.IsAdmin || !employeeId.HasValue)
            {
                var allUpcoming = await _uow.Perf.OneOnOneMeetings.GetUpcomingAsync();
                var allDtos = allUpcoming.Adapt<IEnumerable<OneOnOneMeetingDto>>();
                SanitizePrivateNotes(allDtos, employeeId);
                return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(allDtos, OneOnOneMeetingMsg.RetrievedUpcoming);
            }

            var directReports = await _uow.Info.EmployeeEmployments
                .FindAllAsync(e => e.DirectManagerId == employeeId.Value && !e.IsDeleted);
            var directReportIds = directReports.Select(e => e.EmployeeId).ToHashSet();

            var meetings = await _uow.Perf.OneOnOneMeetings.GetUpcomingAsync();
            var filtered = meetings.Where(m =>
                m.EmployeeId == employeeId.Value ||
                m.ManagerId == employeeId.Value ||
                directReportIds.Contains(m.EmployeeId));
            var dtos = filtered.Adapt<IEnumerable<OneOnOneMeetingDto>>();
            SanitizePrivateNotes(dtos, employeeId);
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedUpcoming);
        }

        public async Task<SuccessResponse<OneOnOneMeetingDto>> GetByIdAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse<OneOnOneMeetingDto>.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            var dto = meeting.Adapt<OneOnOneMeetingDto>();
            var employeeId = await _currentEmployeeContext.GetEmployeeIdAsync();
            SanitizePrivateNotes(dto, employeeId);
            return SuccessResponse<OneOnOneMeetingDto>.Ok(dto, OneOnOneMeetingMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByEmployeeIdAsync(long employeeId)
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetByEmployeeIdAsync(employeeId);
            var dtos = meetings.Adapt<IEnumerable<OneOnOneMeetingDto>>();
            var currentEmployeeId = await _currentEmployeeContext.GetEmployeeIdAsync();
            SanitizePrivateNotes(dtos, currentEmployeeId);
            return SuccessResponse<IEnumerable<OneOnOneMeetingDto>>.Ok(dtos, OneOnOneMeetingMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByManagerIdAsync(long managerId)
        {
            var meetings = await _uow.Perf.OneOnOneMeetings.GetByManagerIdAsync(managerId);
            var dtos = meetings.Adapt<IEnumerable<OneOnOneMeetingDto>>();
            var currentEmployeeId = await _currentEmployeeContext.GetEmployeeIdAsync();
            SanitizePrivateNotes(dtos, currentEmployeeId);
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

            await SendMeetingNotification(
                dto.EmployeeId,
                "New One-on-One Meeting Scheduled",
                $"A new meeting \"{dto.Title}\" has been scheduled for you.",
                "Info",
                "/performance/one-on-one-meetings");

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

            await SendMeetingNotification(
                meeting.EmployeeId,
                "Meeting Completed",
                $"The meeting \"{meeting.Title}\" has been completed. Please review and acknowledge.",
                "Success",
                "/performance/one-on-one-meetings");

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

            await SendMeetingNotification(
                meeting.EmployeeId,
                "Meeting Cancelled",
                $"The meeting \"{meeting.Title}\" has been cancelled.",
                "Error",
                "/performance/one-on-one-meetings");

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

            await SendMeetingNotification(
                meeting.ManagerId,
                "Meeting Acknowledged",
                $"The completion of \"{meeting.Title}\" has been acknowledged by the employee.",
                "Info",
                "/performance/one-on-one-meetings");

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Acknowledged);
        }

        public async Task<SuccessResponse> ConfirmAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (!await IsOwnerAsync(meeting.EmployeeId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            meeting.ConfirmByEmployee();

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            await SendMeetingNotification(
                meeting.ManagerId,
                "Meeting Confirmed",
                $"The meeting \"{meeting.Title}\" has been confirmed by the employee.",
                "Success",
                "/performance/one-on-one-meetings");

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Confirmed);
        }

        public async Task<SuccessResponse> RescheduleByEmployeeAsync(long id, RescheduleMeetingDto dto)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (!await IsOwnerAsync(meeting.EmployeeId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            meeting.RescheduleByEmployee(dto.ScheduledDate, dto.ScheduledEndTime);

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            await SendMeetingNotification(
                meeting.ManagerId,
                "Meeting Rescheduled",
                $"A new time has been proposed for \"{meeting.Title}\" by the employee.",
                "Warning",
                "/performance/one-on-one-meetings");

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Rescheduled);
        }

        public async Task<SuccessResponse> AcceptRescheduleAsync(long id)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (!await IsManagerOrAdminAsync(meeting.ManagerId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            meeting.AcceptRescheduleByManager();

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            await SendMeetingNotification(
                meeting.EmployeeId,
                "Reschedule Accepted",
                $"The reschedule for \"{meeting.Title}\" has been accepted.",
                "Success",
                "/performance/one-on-one-meetings");

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Confirmed);
        }

        public async Task<SuccessResponse> RescheduleByManagerAsync(long id, RescheduleMeetingDto dto)
        {
            var meeting = await _uow.Perf.OneOnOneMeetings.GetByIdAsync(id);

            if (meeting == null)
                return SuccessResponse.Fail(OneOnOneMeetingMsg.NotFound(id), ErrorType.NotFound);

            if (!await IsManagerOrAdminAsync(meeting.ManagerId))
                return SuccessResponse.Fail(OneOnOneMeetingMsg.Unauthorized, ErrorType.Forbidden);

            meeting.RescheduleByManager(dto.ScheduledDate, dto.ScheduledEndTime);

            _uow.Perf.OneOnOneMeetings.Update(meeting);
            await _uow.CompleteAsync();

            await SendMeetingNotification(
                meeting.EmployeeId,
                "New Time Proposed",
                $"A new time has been proposed for \"{meeting.Title}\" by your manager.",
                "Warning",
                "/performance/one-on-one-meetings");

            return SuccessResponse.Ok(OneOnOneMeetingMsg.Rescheduled);
        }
    }
}
