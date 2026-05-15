using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;
using static EPMS.Shared.Constants.FeedbackVisibility;

using Mapster;
namespace EPMS.Domain.Services.Performance
{
    public class ContinuousFeedbackService : IContinuousFeedbackService
    {
        private readonly IUnitOfWork _uow;
                private readonly TimeProvider _timeProvider;

        public ContinuousFeedbackService(IUnitOfWork uow, TimeProvider timeProvider)
        {
            _uow = uow;
            _timeProvider = timeProvider;
        }

        public async Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetAllAsync()
        {
            var feedbacks = await _uow.Perf.ContinuousFeedbacks.GetAllAsync();
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
            var feedback = new ContinuousFeedback(
                dto.EmployeeId,
                dto.GivenById,
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
