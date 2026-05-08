using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.SharedDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Shared;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentAttachmentsController : ApiControllerBase
{
    private readonly IDocumentAttachmentService _documentAttachmentService;

    public DocumentAttachmentsController(IDocumentAttachmentService documentAttachmentService)
    {
        _documentAttachmentService = documentAttachmentService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<DocumentAttachmentDto>>>> GetAll()
    {
        var result = await _documentAttachmentService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SuccessResponse<DocumentAttachmentDto>>> GetById(long id)
    {
        var result = await _documentAttachmentService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("entity/{entityType}/{entityId}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<DocumentAttachmentDto>>>> GetByEntityId(string entityType, long entityId)
    {
        var result = await _documentAttachmentService.GetByEntityIdAsync(entityType, entityId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateDocumentAttachmentDto dto)
    {
        var result = await _documentAttachmentService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateDocumentAttachmentDto dto)
    {
        var result = await _documentAttachmentService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _documentAttachmentService.DeleteAsync(id);
        return HandleResult(result);
    }
}