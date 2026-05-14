using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class SharedValidationExtensions
{
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

    public static IRuleBuilderOptions<T, string> ApplyEntityTypeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.DocumentAttachment.EntityType.Required)
            .MaximumLength(50).WithMessage(SharedValidationMessages.DocumentAttachment.EntityType.MaxLength);
    }

    public static IRuleBuilderOptions<T, long> ApplyEntityIdRules<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage(SharedValidationMessages.DocumentAttachment.EntityId.Invalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyFileNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.DocumentAttachment.FileName.Required)
            .MaximumLength(255).WithMessage(SharedValidationMessages.DocumentAttachment.FileName.MaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyFilePathRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.DocumentAttachment.FilePath.Required)
            .MaximumLength(500).WithMessage(SharedValidationMessages.DocumentAttachment.FilePath.MaxLength);
    }

    public static IRuleBuilderOptions<T, long> ApplyFileSizeRules<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage(SharedValidationMessages.DocumentAttachment.FileSize.Invalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyMimeTypeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.DocumentAttachment.MimeType.Required)
            .MaximumLength(100).WithMessage(SharedValidationMessages.DocumentAttachment.MimeType.MaxLength);
    }

    public static IRuleBuilderOptions<T, long> ApplyUploadedByIdRules<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage(SharedValidationMessages.DocumentAttachment.UploadedById.Invalid);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalDescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(500).WithMessage(SharedValidationMessages.DocumentAttachment.Description.MaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalCategoryRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(50).WithMessage(SharedValidationMessages.DocumentAttachment.Category.MaxLength);
    }
}