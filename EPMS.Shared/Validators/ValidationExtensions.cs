using System.Linq.Expressions;
using EPMS.Shared.Constants.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyEmailRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AuthValidationMessages.Email.Required)
            .MaximumLength(256).WithMessage(AuthValidationMessages.Email.MaxLength)
            .EmailAddress().WithMessage(AuthValidationMessages.Email.Invalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AuthValidationMessages.Password.Required)
            .MinimumLength(8).WithMessage(AuthValidationMessages.Password.MinimumLength)
            .Must(p => !p.Any(char.IsWhiteSpace)).WithMessage(AuthValidationMessages.Password.NoSpaces)
            .Must(p => p.Any(char.IsUpper)).WithMessage(AuthValidationMessages.Password.RequiresUppercase)
            .Must(p => p.Any(char.IsLower)).WithMessage(AuthValidationMessages.Password.RequiresLowercase)
            .Must(p => p.Any(char.IsDigit)).WithMessage(AuthValidationMessages.Password.RequiresDigit);
    }

    public static IRuleBuilderOptions<T, string> ApplyConfirmMatches<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        Expression<Func<T, string>> matchTo)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AuthValidationMessages.Password.ConfirmRequired)
            .Equal(matchTo).WithMessage(AuthValidationMessages.Password.Mismatch);
    }

    public static IRuleBuilderOptions<T, string> ApplyRefreshTokenRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AuthValidationMessages.Tokens.RefreshTokenRequired);
    }

    public static IRuleBuilderOptions<T, string> ApplyDepartmentCodeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Department.CodeRequired)
            .MaximumLength(20).WithMessage(HrValidationMessages.Department.CodeMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyDepartmentNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Department.NameRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Department.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyTeamNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Team.NameRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Team.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, long> ApplyTeamDepartmentIdRules<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder.GreaterThan(0).WithMessage(HrValidationMessages.Team.DepartmentIdInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyPositionTitleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Position.TitleRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Position.TitleMaxLength);
    }

    public static IRuleBuilderOptions<T, long> ApplyPositionLevelIdRules<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder.GreaterThan(0).WithMessage(HrValidationMessages.Position.LevelIdInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyLevelCodeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Level.CodeRequired)
            .MaximumLength(10).WithMessage(HrValidationMessages.Level.CodeMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyLevelNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Level.NameRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Level.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyLevelOptionalDescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(s => string.IsNullOrWhiteSpace(s) || s.Length <= 250)
            .WithMessage(HrValidationMessages.Level.DescriptionMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyPermissionCodeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PermissionValidationMessages.Code.Required)
            .MaximumLength(50).WithMessage(PermissionValidationMessages.Code.MaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyPermissionNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PermissionValidationMessages.Name.Required)
            .MaximumLength(100).WithMessage(PermissionValidationMessages.Name.MaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyCategoryModuleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.Category.ModuleRequired)
            .MaximumLength(50).WithMessage(SharedValidationMessages.Category.ModuleMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyCategoryCodeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.Category.CodeRequired)
            .MaximumLength(50).WithMessage(SharedValidationMessages.Category.CodeMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyCategoryNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.Category.NameRequired)
            .MaximumLength(100).WithMessage(SharedValidationMessages.Category.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyCategoryOptionalDescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(s => string.IsNullOrWhiteSpace(s) || s.Length <= 250)
            .WithMessage(SharedValidationMessages.Category.DescriptionMaxLength);
    }

    public static IRuleBuilderOptions<T, long?> ApplyCategoryOptionalParentIdRules<T>(this IRuleBuilder<T, long?> ruleBuilder)
    {
        return ruleBuilder
            .Must(pid => !pid.HasValue || pid.Value > 0)
            .WithMessage(SharedValidationMessages.Category.ParentIdInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyTagNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.Tag.NameRequired)
            .MaximumLength(50).WithMessage(SharedValidationMessages.Tag.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyTagOptionalModuleRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(s => s == null || string.IsNullOrWhiteSpace(s) || s.Length <= 50)
            .WithMessage(SharedValidationMessages.Tag.ModuleMaxLength);
    }

    // EmployeeInfo Extension Methods
    public static IRuleBuilderOptions<T, long> ApplyEmployeeIdRules<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder.GreaterThan(0).WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmployeeIdInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyStaffNoRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.StaffNoRequired)
            .MaximumLength(20).WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.StaffNoMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyPersonNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(EmployeeInfoValidationMessages.Common.NameRequired)
            .MaximumLength(100).WithMessage(EmployeeInfoValidationMessages.Common.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyOptionalPersonNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.MaximumLength(100).WithMessage(EmployeeInfoValidationMessages.Common.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyPhoneNumberRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(20).WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberMaxLength)
            .Matches(@"^[+]?[\d\s\-\(\)]+$").WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyOptionalPhoneNumberRules<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        Func<T, string?> propertySelector)
    {
        return ruleBuilder
            .MaximumLength(20).WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberMaxLength)
            .Matches(@"^[+]?[\d\s\-\(\)]+$").WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberInvalid)
            .When(x => !string.IsNullOrWhiteSpace(propertySelector(x)));
    }

    public static IRuleBuilderOptions<T, string> ApplyEmailAddressRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(100).WithMessage(EmployeeInfoValidationMessages.EmployeeContact.EmailAddressMaxLength)
            .EmailAddress().WithMessage(EmployeeInfoValidationMessages.EmployeeContact.EmailAddressInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyOptionalEmailAddressRules<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            Func<T, string?> propertySelector)
    {
        return ruleBuilder
            .MaximumLength(100).WithMessage(EmployeeInfoValidationMessages.EmployeeContact.EmailAddressMaxLength)
            .EmailAddress().WithMessage(EmployeeInfoValidationMessages.EmployeeContact.EmailAddressInvalid)
            .When(x => !string.IsNullOrWhiteSpace(propertySelector(x)));
    }

    public static IRuleBuilderOptions<T, string> ApplyAddressRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.MaximumLength(500).WithMessage(EmployeeInfoValidationMessages.EmployeeContact.ContactAddressMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyNRCRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.MaximumLength(50).WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.NRCMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyCurrencyRules<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            Func<T, string?> propertySelector)
    {
        return ruleBuilder
            .MaximumLength(10).WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.CurrencyMaxLength)
            .Matches(@"^[A-Z]{3}$").WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.CurrencyInvalid)
            .When(x => !string.IsNullOrWhiteSpace(propertySelector(x)));
    }

    public static IRuleBuilderOptions<T, decimal> ApplySalaryRules<T>(this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder.GreaterThanOrEqualTo(0).WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.SalaryInvalid);
    }

    public static IRuleBuilderOptions<T, DateOnly> ApplyDateOfBirthRules<T>(
            this IRuleBuilder<T, DateOnly> ruleBuilder,
            TimeProvider timeProvider)
    {
        return ruleBuilder
            .LessThanOrEqualTo(x => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.DateOfBirthFuture);
    }

    public static IRuleBuilderOptions<T, DateOnly?> ApplyOptionalDateOfBirthRules<T>(
            this IRuleBuilder<T, DateOnly?> ruleBuilder,
            TimeProvider timeProvider,
            Func<T, DateOnly?> propertySelector)
    {
        return ruleBuilder
            .LessThanOrEqualTo(x => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.DateOfBirthFuture)
            .When(x => propertySelector(x).HasValue);
    }

    public static IRuleBuilderOptions<T, DateOnly> ApplyFutureDatePreventionRules<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
    {
        return ruleBuilder.LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(EmployeeInfoValidationMessages.Common.DateFuture);
    }

    public static IRuleBuilderOptions<T, DateOnly?> ApplyOptionalFutureDatePreventionRules<T>(
            this IRuleBuilder<T, DateOnly?> ruleBuilder,
            TimeProvider timeProvider,
            Func<T, DateOnly?> propertySelector)
    {
        return ruleBuilder
            .LessThanOrEqualTo(x => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime))
            .WithMessage(EmployeeInfoValidationMessages.Common.DateFuture)
            .When(x => propertySelector(x).HasValue);
    }

    public static IRuleBuilderOptions<T, string> ApplyHexColorCodeRules<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            Func<T, string?> propertySelector)
    {
        return ruleBuilder
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage(PerformanceValidationMessages.KPIWeightPriority.ColorCodeInvalid)
            .When(x => !string.IsNullOrWhiteSpace(propertySelector(x)));
    }

    // Performance Extension Methods
    public static IRuleBuilderOptions<T, int> ApplyRatingRules<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder.GreaterThan(0).WithMessage(PerformanceValidationMessages.RatingScale.RatingInvalid);
    }

    public static IRuleBuilderOptions<T, decimal> ApplyScoreRangeRules<T>(this IRuleBuilder<T, decimal> ruleBuilder, string scoreType)
    {
        var message = scoreType switch
        {
            "Minimum" => PerformanceValidationMessages.ScoreRange.MinimumScoreInvalid,
            "Maximum" => PerformanceValidationMessages.ScoreRange.MaximumScoreInvalid,
            _ => PerformanceValidationMessages.ScoreRange.MinimumScoreInvalid
        };
        return ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage(message);
    }

    public static IRuleBuilderOptions<T, decimal?> ApplyOptionalScoreRangeRules<T>(
    this IRuleBuilder<T, decimal?> ruleBuilder,
    string scoreType,
    Func<T, decimal?> propertySelector)
    {
        var message = scoreType switch
        {
            "Minimum" => PerformanceValidationMessages.ScoreRange.MinimumScoreInvalid,
            "Maximum" => PerformanceValidationMessages.ScoreRange.MaximumScoreInvalid,
            _ => PerformanceValidationMessages.ScoreRange.MinimumScoreInvalid
        };
        return ruleBuilder
            .GreaterThanOrEqualTo(0)
            .WithMessage(message)
            .When(x => propertySelector(x).HasValue);
    }

    public static IRuleBuilderOptions<T, decimal> ApplyWeightRangeRules<T>(this IRuleBuilder<T, decimal> ruleBuilder, string weightType)
    {
        var message = weightType switch
        {
            "Minimum" => PerformanceValidationMessages.WeightRange.MinimumWeightInvalid,
            "Maximum" => PerformanceValidationMessages.WeightRange.MaximumWeightInvalid,
            _ => PerformanceValidationMessages.WeightRange.MinimumWeightInvalid
        };
        return ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage(message);
    }

    public static IRuleBuilderOptions<T, decimal?> ApplyOptionalyWeightRangeRules<T>(
    this IRuleBuilder<T, decimal?> ruleBuilder,
    string weightType,
    Func<T, decimal?> propertySelector)
    {
        var message = weightType switch
        {
            "Minimum" => PerformanceValidationMessages.WeightRange.MinimumWeightInvalid,
            "Maximum" => PerformanceValidationMessages.WeightRange.MaximumWeightInvalid,
            _ => PerformanceValidationMessages.WeightRange.MinimumWeightInvalid
        };
        return ruleBuilder
            .GreaterThanOrEqualTo(0)
            .WithMessage(message)
            .When(x => propertySelector(x).HasValue);
    }

    public static IRuleBuilderOptions<T, string> ApplyPerformanceLevelNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PerformanceValidationMessages.KPIWeightPriority.LevelNameRequired)
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.KPIWeightPriority.LevelNameMaxLength);
    }
}
