namespace EPMS.Domain.Interface.IService.App
{
    public interface ICurrentUserService
    {
        long? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
    }
}
