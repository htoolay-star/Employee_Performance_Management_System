namespace EPMS.Shared.DTOs.Auth
{
    public record CreateUserRequest
    {
        public string Email { get; init; } = string.Empty;
        public string StaffNo { get; init; } = string.Empty;
        public string StaffName { get; init; } = string.Empty;
        public long PositionId { get; init; }
    }
}
