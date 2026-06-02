using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.DTOs.AppDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.App;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _timeProvider;

    public NotificationService(IUnitOfWork uow, TimeProvider timeProvider)
    {
        _uow = uow;
        _timeProvider = timeProvider;
    }

    public async Task<SuccessResponse<IEnumerable<NotificationDto>>> GetAllByUserIdAsync(long userId)
    {
        var notifications = await _uow.App.Notifications.GetByUserIdAsync(userId);
        var dtos = notifications.Adapt<IEnumerable<NotificationDto>>();
        return SuccessResponse<IEnumerable<NotificationDto>>.Ok(dtos, NotificationMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<NotificationDto>>> GetUnreadByUserIdAsync(long userId)
    {
        var notifications = await _uow.App.Notifications.GetUnreadByUserIdAsync(userId);
        var dtos = notifications.Adapt<IEnumerable<NotificationDto>>();
        return SuccessResponse<IEnumerable<NotificationDto>>.Ok(dtos, NotificationMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<NotificationDto>> GetByIdAsync(long id)
    {
        var notification = await _uow.App.Notifications.GetByIdAsync(id);

        if (notification == null)
            return SuccessResponse<NotificationDto>.Fail(NotificationMsg.NotFound(id), ErrorType.NotFound);

        var dto = notification.Adapt<NotificationDto>();
        return SuccessResponse<NotificationDto>.Ok(dto, NotificationMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateNotificationDto dto)
    {
        var notification = new Notification(
            dto.ToUserId,
            dto.Title,
            dto.Message,
            dto.Type,
            _timeProvider,
            dto.RedirectUrl);

        _uow.App.Notifications.Add(notification);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(notification.Id, NotificationMsg.Created);
    }

    public async Task<SuccessResponse> MarkAsReadAsync(long id)
    {
        var notification = await _uow.App.Notifications.GetByIdAsync(id);

        if (notification == null)
            return SuccessResponse.Fail(NotificationMsg.NotFound(id), ErrorType.NotFound);

        notification.MarkAsRead(_timeProvider);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(NotificationMsg.MarkedAsRead);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var notification = await _uow.App.Notifications.GetByIdAsync(id);

        if (notification == null)
            return SuccessResponse.Fail(NotificationMsg.NotFound(id), ErrorType.NotFound);

        _uow.App.Notifications.Delete(notification);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(NotificationMsg.Deleted);
    }

    public async Task<SuccessResponse> MarkAllAsReadAsync(long userId)
    {
        await _uow.App.Notifications.MarkAllAsReadAsync(userId);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(NotificationMsg.MarkedAllAsRead);
    }
}
