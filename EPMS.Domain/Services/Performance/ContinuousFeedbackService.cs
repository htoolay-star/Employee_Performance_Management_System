using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Performance
{
    public class ContinuousFeedbackService : IContinuousFeedbackService
    {
        private readonly IUnitOfWork _uow;
        private readonly TimeProvider _timeProvider;
        private readonly ICurrentEmployeeContextService _currentEmployee;

        public ContinuousFeedbackService(IUnitOfWork uow, TimeProvider timeProvider, ICurrentEmployeeContextService currentEmployee)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _currentEmployee = currentEmployee;
        }

        public async Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetAllAsync()
        {
            var viewerEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
            var isAdmin = _currentEmployee.IsAdmin;

            var feedbacks = await _uow.Perf.ContinuousFeedbacks.GetAllWithIncludesAsync();

            if (!isAdmin && viewerEmployeeId.HasValue)
            {
                var directReports = await _uow.Info.EmployeeEmployments
                    .FindAllAsync(e => e.DirectManagerId == viewerEmployeeId.Value && !e.IsDeleted);
                var directReportIds = directReports.Select(e => e.EmployeeId).ToHashSet();

                feedbacks = feedbacks.Where(f =>
                    f.GivenById == viewerEmployeeId
                    || (f.Visibility == FeedbackVisibility.Public && f.EmployeeId == viewerEmployeeId)
                    || (f.Visibility == FeedbackVisibility.ManagerOnly && directReportIds.Contains(f.EmployeeId))
                    || (f.Visibility == FeedbackVisibility.AdminOnly && directReportIds.Contains(f.EmployeeId))
                );
            }

            var dtos = feedbacks.Adapt<IEnumerable<ContinuousFeedbackDto>>();
            return SuccessResponse<IEnumerable<ContinuousFeedbackDto>>.Ok(dtos, ContinuousFeedbackMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetReceivedFeedbackAsync()
        {
            var viewerEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
            var isAdmin = _currentEmployee.IsAdmin;

            if (!viewerEmployeeId.HasValue)
                return SuccessResponse<IEnumerable<ContinuousFeedbackDto>>.Ok(
                    Enumerable.Empty<ContinuousFeedbackDto>(), ContinuousFeedbackMsg.RetrievedAll);

            var feedbacks = await _uow.Perf.ContinuousFeedbacks.GetAllWithIncludesAsync();

            if (isAdmin)
            {
                feedbacks = feedbacks.Where(f =>
                    (f.EmployeeId == viewerEmployeeId.Value && f.Visibility == FeedbackVisibility.Public)
                    || f.Visibility == FeedbackVisibility.AdminOnly
                );
            }
            else
            {
                var directReports = await _uow.Info.EmployeeEmployments
                    .FindAllAsync(e => e.DirectManagerId == viewerEmployeeId.Value && !e.IsDeleted);
                var directReportIds = directReports.Select(e => e.EmployeeId).ToHashSet();

                feedbacks = feedbacks.Where(f =>
                    (f.EmployeeId == viewerEmployeeId.Value && f.Visibility == FeedbackVisibility.Public)
                    || (directReportIds.Contains(f.EmployeeId) && f.Visibility == FeedbackVisibility.ManagerOnly)
                );
            }

            var dtos = feedbacks.Adapt<IEnumerable<ContinuousFeedbackDto>>();
            return SuccessResponse<IEnumerable<ContinuousFeedbackDto>>.Ok(dtos, ContinuousFeedbackMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetGivenFeedbackAsync()
        {
            var viewerEmployeeId = await _currentEmployee.GetEmployeeIdAsync();

            if (!viewerEmployeeId.HasValue)
                return SuccessResponse<IEnumerable<ContinuousFeedbackDto>>.Ok(
                    Enumerable.Empty<ContinuousFeedbackDto>(), ContinuousFeedbackMsg.RetrievedAll);

            var feedbacks = await _uow.Perf.ContinuousFeedbacks.GetAllWithIncludesAsync();

            feedbacks = feedbacks.Where(f => f.GivenById == viewerEmployeeId.Value);

            var dtos = feedbacks.Adapt<IEnumerable<ContinuousFeedbackDto>>();
            return SuccessResponse<IEnumerable<ContinuousFeedbackDto>>.Ok(dtos, ContinuousFeedbackMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<ContinuousFeedbackDto>> GetByIdAsync(long id)
        {
            var feedback = await _uow.Perf.ContinuousFeedbacks.GetByIdAsync(id);

            if (feedback == null)
                return SuccessResponse<ContinuousFeedbackDto>.Fail(ContinuousFeedbackMsg.NotFound(id), ErrorType.NotFound);

            var dto = feedback.Adapt<ContinuousFeedbackDto>();
            return SuccessResponse<ContinuousFeedbackDto>.Ok(dto, ContinuousFeedbackMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetByEmployeeIdAsync(long employeeId)
        {
            var feedbacks = await _uow.Perf.ContinuousFeedbacks.GetByEmployeeIdAsync(employeeId);
            var dtos = feedbacks.Adapt<IEnumerable<ContinuousFeedbackDto>>();
            return SuccessResponse<IEnumerable<ContinuousFeedbackDto>>.Ok(dtos, ContinuousFeedbackMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetByUserIdAsync(long userId)
        {
            var feedbacks = await _uow.Perf.ContinuousFeedbacks.GetGivenByUserIdAsync(userId);
            var dtos = feedbacks.Adapt<IEnumerable<ContinuousFeedbackDto>>();
            return SuccessResponse<IEnumerable<ContinuousFeedbackDto>>.Ok(dtos, ContinuousFeedbackMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateContinuousFeedbackDto dto)
        {
            var isAdmin = _currentEmployee.IsAdmin;
            var viewerEmployeeId = await _currentEmployee.GetEmployeeIdAsync();

            if (!viewerEmployeeId.HasValue)
                return SuccessResponse<long>.Fail("User identity not found.", ErrorType.Unauthorized);

            if (isAdmin)
            {
                if (dto.Visibility == FeedbackVisibility.AdminOnly)
                    return SuccessResponse<long>.Fail("Admins cannot set Admin Only visibility.", ErrorType.Validation);
            }
            else
            {
                var hasDirectReports = await _uow.Info.EmployeeEmployments
                    .AnyAsync(e => e.DirectManagerId == viewerEmployeeId.Value && !e.IsDeleted);

                if (!hasDirectReports)
                    return SuccessResponse<long>.Fail("Only admins and managers can create feedback.", ErrorType.Validation);

                if (dto.Visibility == FeedbackVisibility.ManagerOnly)
                    return SuccessResponse<long>.Fail("Only admins can set Manager Only visibility.", ErrorType.Validation);
            }

            var feedback = new ContinuousFeedback(
                dto.EmployeeId,
                viewerEmployeeId.Value,
                dto.FeedbackType,
                dto.Content,
                _timeProvider,
                dto.Visibility,
                dto.RelatedGoalId);

            _uow.Perf.ContinuousFeedbacks.Add(feedback);
            await _uow.CompleteAsync();

            return SuccessResponse<long>.Ok(feedback.Id, ContinuousFeedbackMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateContinuousFeedbackDto dto)
        {
            var feedback = await _uow.Perf.ContinuousFeedbacks.GetByIdAsync(id);

            if (feedback == null)
                return SuccessResponse.Fail(ContinuousFeedbackMsg.NotFound(id), ErrorType.NotFound);

            feedback.Update(dto.Content, dto.Visibility);

            _uow.Perf.ContinuousFeedbacks.Update(feedback);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(ContinuousFeedbackMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var feedback = await _uow.Perf.ContinuousFeedbacks.GetByIdAsync(id);

            if (feedback == null)
                return SuccessResponse.Fail(ContinuousFeedbackMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.ContinuousFeedbacks.Delete(feedback);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(ContinuousFeedbackMsg.Deleted);
        }
    }
}
