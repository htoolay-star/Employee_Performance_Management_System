namespace EPMS.Shared.DTOs.AuthDTOs
{
    /// <summary>
    /// DTO for caching user data in distributed cache.
    /// Excludes sensitive navigation properties and includes only essential fields.
    /// </summary>
    public class CachedUserInfo
    {
        public long Id { get; set; }
        public Guid PublicId { get; set; }
        public string Email { get; set; } = null!;
        public string? RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsFirstLogin { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }
        public string PasswordHash { get; set; } = null!;
    }
}
