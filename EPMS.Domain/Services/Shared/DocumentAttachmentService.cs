using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.SharedDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Shared;

public class DocumentAttachmentService : IDocumentAttachmentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;

    public DocumentAttachmentService(IUnitOfWork uow, IMapper mapper, TimeProvider timeProvider)
    {
        _uow = uow;
        _mapper = mapper;
        _timeProvider = timeProvider;
    }

    public async Task<SuccessResponse<IEnumerable<DocumentAttachmentDto>>> GetAllAsync()
    {
        var attachments = await _uow.Shared.DocumentAttachments.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<DocumentAttachmentDto>>(attachments);
        return SuccessResponse<IEnumerable<DocumentAttachmentDto>>.Ok(dtos, DocumentAttachmentMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<DocumentAttachmentDto>> GetByIdAsync(long id)
    {
        var attachment = await _uow.Shared.DocumentAttachments.GetByIdAsync(id);

        if (attachment == null)
            return SuccessResponse<DocumentAttachmentDto>.Fail(DocumentAttachmentMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<DocumentAttachmentDto>(attachment);
        return SuccessResponse<DocumentAttachmentDto>.Ok(dto, DocumentAttachmentMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<DocumentAttachmentDto>>> GetByEntityIdAsync(string entityType, long entityId)
    {
        var attachments = await _uow.Shared.DocumentAttachments.GetByEntityIdAsync(entityType, entityId);
        var dtos = _mapper.Map<IEnumerable<DocumentAttachmentDto>>(attachments);
        return SuccessResponse<IEnumerable<DocumentAttachmentDto>>.Ok(dtos, DocumentAttachmentMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateDocumentAttachmentDto dto)
    {
        var attachment = new DocumentAttachment(
            dto.EntityType,
            dto.EntityId,
            dto.FileName,
            dto.FilePath,
            dto.FileSize,
            dto.MimeType,
            dto.UploadedById,
            _timeProvider,
            dto.Description,
            dto.Category);

        _uow.Shared.DocumentAttachments.Add(attachment);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(attachment.Id, DocumentAttachmentMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateDocumentAttachmentDto dto)
    {
        var attachment = await _uow.Shared.DocumentAttachments.GetByIdAsync(id);

        if (attachment == null)
            return SuccessResponse.Fail(DocumentAttachmentMsg.NotFound(id), ErrorType.NotFound);

        if (dto.Description != null)
            attachment.UpdateDescription(dto.Description);

        if (dto.Category != null)
            attachment.UpdateCategory(dto.Category);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(DocumentAttachmentMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var attachment = await _uow.Shared.DocumentAttachments.GetByIdAsync(id);

        if (attachment == null)
            return SuccessResponse.Fail(DocumentAttachmentMsg.NotFound(id), ErrorType.NotFound);

        _uow.Shared.DocumentAttachments.Delete(attachment);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(DocumentAttachmentMsg.Deleted);
    }
}