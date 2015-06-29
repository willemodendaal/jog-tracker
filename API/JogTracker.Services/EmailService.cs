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
            var smtpClient = new SmtpClient(GlobalConfig.SendGridSmtpServer);
            var mail = new MailMessage(GlobalConfig.JogTrackerEmail, address);
            mail.Subject = subject;
            mail.Body = body;

            smtpClient.Credentials = new NetworkCredential(GlobalConfig.SendGridUser, GlobalConfig.SendGridPassword);
            smtpClient.Send(mail);
        }

        public string GetResetPasswordEmailBody(string userId, string resetToken, string userName)
        {
            string emailBody = new ResetEmailBodyGenerator().GetEmailBody(userName, userId, resetToken);
            return emailBody;
        }

    }
}
