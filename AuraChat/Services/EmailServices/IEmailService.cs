using AuraChat.Models;

namespace AuraChat.Services.EmailServices;

public interface IEmailService
{
    public void SendEmail(EmailModel emailModel);
}
