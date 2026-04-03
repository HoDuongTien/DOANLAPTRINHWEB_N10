using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    public EmailSender(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var message = new MailMessage();

        message.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
        message.To.Add(toEmail);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port);
        client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
        client.EnableSsl = true;

        await client.SendMailAsync(message);
    }
}
