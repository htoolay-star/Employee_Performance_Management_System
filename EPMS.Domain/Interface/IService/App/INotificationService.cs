using EPMS.Shared.DTOs.AppDTOs;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.App;

public interface INotificationService
{
    Task<SuccessResponse<IEnumerable<NotificationDto>>> GetAllByUserIdAsync(long userId);
    Task<SuccessResponse<IEnumerable<NotificationDto>>> GetUnreadByUserIdAsync(long userId);
    Task<SuccessResponse<NotificationDto>> GetByIdAsync(long id);
    Task<SuccessResponse<long>> CreateAsync(CreateNotificationDto dto);
    Task<SuccessResponse> MarkAsReadAsync(long id);
    Task<SuccessResponse> MarkAllAsReadAsync(long userId);
    Task<SuccessResponse> DeleteAsync(long id);
}