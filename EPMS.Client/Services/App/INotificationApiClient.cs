using EPMS.Shared.DTOs.AppDTOs;
using EPMS.Shared.DTOs.Common;
using Refit;

namespace EPMS.Client.Services.App;

public interface INotificationApiClient
{
    [Get("/api/Notifications/list")]
    Task<SuccessResponse<IEnumerable<NotificationDto>>> GetMyNotificationsAsync();

    [Get("/api/Notifications/unread")]
    Task<SuccessResponse<IEnumerable<NotificationDto>>> GetMyUnreadAsync();

    [Put("/api/Notifications/{id}/read")]
    Task<SuccessResponse> MarkAsReadAsync(long id);

    [Delete("/api/Notifications/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
