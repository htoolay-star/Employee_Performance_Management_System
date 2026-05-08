using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.OneOnOneMeetingDTOs
{
    public class CreateOneOnOneMeetingValidator : AbstractValidator<CreateOneOnOneMeetingDto>
    {
        public CreateOneOnOneMeetingValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.OneOnOneMeeting.EmployeeIdRequired);

            RuleFor(x => x.ManagerId)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.OneOnOneMeeting.ManagerIdRequired);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(PerformanceValidationMessages.OneOnOneMeeting.TitleRequired)
                .MaximumLength(200).WithMessage(PerformanceValidationMessages.OneOnOneMeeting.TitleMaxLength);

            RuleFor(x => x.ScheduledDate)
                .NotEmpty()
                .WithMessage(PerformanceValidationMessages.OneOnOneMeeting.ScheduledDateRequired);
        }
    }
}