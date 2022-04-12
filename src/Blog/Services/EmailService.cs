
using Blog.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Blog.Services;

public class EmailService : IEmailService
{
    public async Task<bool> SendAsync(string toName, string toEmail, string subject, string body, string fromName, string fromEmail)
    {
        using (var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port))
        {
            smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromEmail, fromName);
            mailMessage.To.Add(new MailAddress(toEmail, toName));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}
