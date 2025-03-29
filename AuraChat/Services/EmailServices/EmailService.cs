using AuraChat.Models;
using System.Net.Mail;
using System.Net;

namespace AuraChat.Services.EmailServices;

public class EmailService : IEmailService
{

    public void SendEmail(EmailModel emailModel)
    {
        using (MailMessage mail = new())
        {
            var email = Environment.GetEnvironmentVariable("BusniessAccountEmail") ?? throw new Exception("email field not found in enviroment");
            mail.From = new MailAddress(email);
            mail.To.Add(emailModel.ReicieverEmail);
            mail.Subject = emailModel.Header;
            mail.Body = emailModel.Body;
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(email, Environment.GetEnvironmentVariable("BusinessAccountPassword") ?? throw new Exception("password field not found in enviroment"));
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
    }
}