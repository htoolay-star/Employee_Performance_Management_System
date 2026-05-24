namespace EPMS.Shared.DTOs.Auth;

public record VerifyOtpRequest
{
    public string Email { get; init; } = string.Empty;
    public string Otp { get; init; } = string.Empty;
}
