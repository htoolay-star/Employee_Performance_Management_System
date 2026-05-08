using EPMS.Shared.DTOs.SharedDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.SharedDTOs;

public class UpdateDocumentAttachmentValidator : AbstractValidator<UpdateDocumentAttachmentDto>
{
    public UpdateDocumentAttachmentValidator()
    {
        RuleFor(x => x.Description).ApplyOptionalDescriptionRules();
        RuleFor(x => x.Category).ApplyOptionalCategoryRules();
    }
}