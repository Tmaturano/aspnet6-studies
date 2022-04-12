namespace Blog.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendAsync(string toName,
              string toEmail,
              string subject,
              string body,
              string fromName,
              string fromEmail);
}
