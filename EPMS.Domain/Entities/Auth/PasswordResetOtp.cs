using EPMS.Domain.Contracts;

namespace EPMS.Domain.Entities.Auth;

public class PasswordResetOtp : BaseEntity
{
    private PasswordResetOtp() { }

    public PasswordResetOtp(string email, string otp, DateTimeOffset expiresAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(otp);

        Email = email;
        Otp = otp;
        ExpiresAt = expiresAt;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public string Email { get; private set; } = string.Empty;
    public string Otp { get; private set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UsedAt { get; private set; }

    public bool IsValid(TimeProvider timeProvider) =>
        !IsUsed && timeProvider.GetUtcNow() < ExpiresAt;

    public void MarkAsUsed(TimeProvider timeProvider)
    {
        IsUsed = true;
        UsedAt = timeProvider.GetUtcNow();
    }
}
