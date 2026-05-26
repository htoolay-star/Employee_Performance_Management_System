using EPMS.Shared.DTOs.AppDTOs;
using EPMS.Shared.DTOs.Common;
using Refit;

namespace EPMS.Client.Services.App;

public interface INotificationApiClient
{
    [Get("/api/Notifications/user/{userId}")]
    Task<SuccessResponse<IEnumerable<NotificationDto>>> GetByUserIdAsync(long userId);

    [Get("/api/Notifications/unread/{userId}")]
    Task<SuccessResponse<IEnumerable<NotificationDto>>> GetUnreadAsync(long userId);

    [Put("/api/Notifications/{id}/read")]
    Task<SuccessResponse> MarkAsReadAsync(long id);

    [Delete("/api/Notifications/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
