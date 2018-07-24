using System.Net.Mail;

namespace Ex2.Infrastructure.Interfaces
{
    public interface IMailSender
    {
        void Send(MailMessage mailMessage);
    }
}