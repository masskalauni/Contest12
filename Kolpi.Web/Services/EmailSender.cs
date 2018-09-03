using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Kolpi.Web.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (SmtpClient client = new SmtpClient("server_name"))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("username", "password");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(email);
                mailMessage.To.Add(email);
                mailMessage.Body = htmlMessage;
                mailMessage.Subject = subject;
                client.Send(mailMessage);
            }

            return Task.FromResult(0);
        }
    }
}