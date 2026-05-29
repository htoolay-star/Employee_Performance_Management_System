using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.DTOs.AppDTOs;
using EPMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EPMS.Api.Controllers.App;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : ApiControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<NotificationDto>>>> GetMyNotifications()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();
        var result = await _notificationService.GetAllByUserIdAsync(userId.Value);
        return HandleResult(result);
    }

    [HttpGet("unread")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<NotificationDto>>>> GetMyUnread()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();
        var result = await _notificationService.GetUnreadByUserIdAsync(userId.Value);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SuccessResponse<NotificationDto>>> GetById(long id)
    {
        var result = await _notificationService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateNotificationDto dto)
    {
        var result = await _notificationService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id}/read")]
    public async Task<ActionResult<SuccessResponse>> MarkAsRead(long id)
    {
        var result = await _notificationService.MarkAsReadAsync(id);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _notificationService.DeleteAsync(id);
        return HandleResult(result);
    }

    private long? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (long.TryParse(claim, out var id))
            return id;
        return null;
    }
}