using EPMS.Shared.DTOs.SharedDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.SharedDTOs;

public class CreateDocumentAttachmentValidator : AbstractValidator<CreateDocumentAttachmentDto>
{
    public CreateDocumentAttachmentValidator()
    {
        RuleFor(x => x.EntityType).ApplyEntityTypeRules();
        RuleFor(x => x.EntityId).ApplyEntityIdRules();
        RuleFor(x => x.FileName).ApplyFileNameRules();
        RuleFor(x => x.FilePath).ApplyFilePathRules();
        RuleFor(x => x.FileSize).ApplyFileSizeRules();
        RuleFor(x => x.MimeType).ApplyMimeTypeRules();
        RuleFor(x => x.UploadedById).ApplyUploadedByIdRules();
        RuleFor(x => x.Description).ApplyOptionalDescriptionRules();
        RuleFor(x => x.Category).ApplyOptionalCategoryRules();
    }
}