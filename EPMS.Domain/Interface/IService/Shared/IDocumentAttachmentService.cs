using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.SharedDTOs;

namespace EPMS.Domain.Interface.IService.Shared;

public interface IDocumentAttachmentService
{
    Task<SuccessResponse<IEnumerable<DocumentAttachmentDto>>> GetAllAsync();
    Task<SuccessResponse<DocumentAttachmentDto>> GetByIdAsync(long id);
    Task<SuccessResponse<IEnumerable<DocumentAttachmentDto>>> GetByEntityIdAsync(string entityType, long entityId);
    Task<SuccessResponse<long>> CreateAsync(CreateDocumentAttachmentDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateDocumentAttachmentDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}