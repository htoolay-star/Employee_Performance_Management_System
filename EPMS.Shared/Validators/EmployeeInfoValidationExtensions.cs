using EPMS.Shared.Constants;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;
using System;

namespace EPMS.Shared.Validators;

public static class EmployeeInfoValidationExtensions
{
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

    public static IRuleBuilderOptions<T, string> ApplyEmploymentStatusRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmploymentStatusRequired)
            .MaximumLength(50).WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmploymentStatusMaxLength)
            .Must(status => EmploymentStatuses.All.Contains(status))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmploymentStatusInvalid);
    }
}