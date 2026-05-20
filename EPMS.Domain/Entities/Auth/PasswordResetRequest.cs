using EPMS.Domain.Contracts;

namespace EPMS.Domain.Entities.Auth;

public enum ResetRequestStatus
{
    Pending,
    Approved,
    Rejected
}

public class PasswordResetRequest : BaseEntity
{
    private PasswordResetRequest() { }

    public PasswordResetRequest(long userId, string email)
    {
        UserId = userId;
        Email = email;
        Status = ResetRequestStatus.Pending;
        RequestedAt = DateTimeOffset.UtcNow;
    }

    public long UserId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public ResetRequestStatus Status { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; }
    public long? ReviewedBy { get; private set; }
    public DateTimeOffset? ReviewedAt { get; private set; }
    public string? RejectionReason { get; private set; }

    public virtual User User { get; private set; } = null!;

    public void Approve(long reviewerId)
    {
        Status = ResetRequestStatus.Approved;
        ReviewedBy = reviewerId;
        ReviewedAt = DateTimeOffset.UtcNow;
    }

    public void Reject(long reviewerId, string? reason = null)
    {
        Status = ResetRequestStatus.Rejected;
        ReviewedBy = reviewerId;
        ReviewedAt = DateTimeOffset.UtcNow;
        RejectionReason = reason;
    }
}
