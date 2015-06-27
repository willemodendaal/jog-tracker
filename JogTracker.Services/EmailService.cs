using JogTracker.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace JogTracker.Services
{
    public interface IEmailService
    {
        Task SendEmailTo(string address, string subject, string body);
        string GetResetPasswordEmailBody(string userId, string resetToken, string userName);
    }

    public class EmailService : IEmailService
    {
        public async Task SendEmailTo(string address, string subject, string body)
        {
            var smtpClient = new SmtpClient(Config.SendGridSmtpServer);
            var mail = new MailMessage(Config.JogTrackerEmail, address);
            mail.Subject = subject;
            mail.Body = body;

            smtpClient.Credentials = new NetworkCredential(Config.SendGridUser, Config.SendGridPassword);
            smtpClient.Send(mail);
        }

        public string GetResetPasswordEmailBody(string userId, string resetToken, string userName)
        {
            return string.Format("Dear {0}\n\nPlease click on this link to reset your password:\n{1}\n\nIf you did not request this email, please ignore it.\n\nRegards,\nThe Jogging Tracker team",
                userName, resetToken);
        }

    }
}
