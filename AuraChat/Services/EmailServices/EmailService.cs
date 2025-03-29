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
            mail.From = new MailAddress("lmsmailproject@gmail.com");
            mail.To.Add(emailModel.ReicieverEmail);
            mail.Subject = emailModel.Header;
            mail.Body = emailModel.Body;
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("lmsmailproject@gmail.com", "zqkdstiwgwiscsvp");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
    }
}