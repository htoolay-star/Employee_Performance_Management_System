using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Models;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;

namespace EPMS.Domain.Services.Auth;

public class EmailService : IEmailService
{
    private readonly MailtrapSettings _settings;

    public EmailService(IOptions<MailtrapSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendOtpAsync(string toEmail, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("EPMS System", _settings.FromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = "Your Password Reset OTP";

        message.Body = new TextPart("html")
        {
            Text = $"""
            <div style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto;">
                <h2>Password Reset Request</h2>
                <p>You requested to reset your password. Use the OTP below to verify your identity:</p>
                <div style="font-size: 32px; font-weight: bold; letter-spacing: 8px; text-align: center;
                            padding: 20px; background: #f5f5f5; border-radius: 8px; margin: 20px 0;">
                    {otp}
                </div>
                <p>This OTP is valid for <strong>10 minutes</strong>.</p>
                <p>If you did not request this, please ignore this email.</p>
                <hr style="margin-top: 20px;">
                <p style="color: #888; font-size: 12px;">EPMS - Employee Performance Management System</p>
            </div>
            """
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.Host, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.Username, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
