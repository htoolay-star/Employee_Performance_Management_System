namespace EPMS.Domain.Interface.IService.Auth;

public interface IEmailService
{
    Task SendOtpAsync(string toEmail, string otp);
}
