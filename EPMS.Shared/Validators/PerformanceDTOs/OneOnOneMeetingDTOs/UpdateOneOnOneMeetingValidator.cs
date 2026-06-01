using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.OneOnOneMeetingDTOs
{
    public class UpdateOneOnOneMeetingValidator : AbstractValidator<UpdateOneOnOneMeetingDto>
    {
        public UpdateOneOnOneMeetingValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.OneOnOneMeeting.IdRequired);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(PerformanceValidationMessages.OneOnOneMeeting.TitleRequired)
                .MaximumLength(200).WithMessage(PerformanceValidationMessages.OneOnOneMeeting.TitleMaxLength);

            RuleFor(x => x.ScheduledDate)
                .NotEmpty()
                .WithMessage(PerformanceValidationMessages.OneOnOneMeeting.ScheduledDateRequired);

            RuleFor(x => x.ScheduledEndTime)
                .NotEmpty()
                .WithMessage(PerformanceValidationMessages.OneOnOneMeeting.ScheduledEndTimeRequired)
                .GreaterThan(x => x.ScheduledDate)
                .WithMessage(PerformanceValidationMessages.OneOnOneMeeting.EndTimeAfterStartTime);
        }
    }
}