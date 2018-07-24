using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ex2.Infrastructure.Interfaces.Wrappers
{
    public interface ISmtpClient
    {
        void Send(string @from, string recipients, string subject, string body);
        void Send(MailMessage message);
        void SendAsync(string @from, string recipients, string subject, string body, object userToken);
        void SendAsync(MailMessage message, object userToken);
        void SendAsyncCancel();
        Task SendMailAsync(string @from, string recipients, string subject, string body);
        Task SendMailAsync(MailMessage message);
        void Dispose();
        string Host { get; set; }
        int Port { get; set; }
        bool UseDefaultCredentials { get; set; }
        ICredentialsByHost Credentials { get; set; }
        int Timeout { get; set; }
        ServicePoint ServicePoint { get; }
        SmtpDeliveryMethod DeliveryMethod { get; set; }
        SmtpDeliveryFormat DeliveryFormat { get; set; }
        string PickupDirectoryLocation { get; set; }
        bool EnableSsl { get; set; }
        X509CertificateCollection ClientCertificates { get; }
        string TargetName { get; set; }
        event SendCompletedEventHandler SendCompleted;
    }
}