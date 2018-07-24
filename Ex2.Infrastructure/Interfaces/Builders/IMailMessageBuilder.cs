using System.IO;
using System.Net.Mail;

namespace Ex2.Infrastructure.Interfaces.Builders
{
    public interface IMailMessageBuilder : IBuilder<MailMessage>
    {
        IMailMessageBuilder Create();

        IMailMessageBuilder AddAttachment(Stream stream, string fileName);

        IMailMessageBuilder SetBody(string messageBody);

        IMailMessageBuilder SetSenderAddress(string mailAddress);

        IMailMessageBuilder SetSubject(string subject);

        IMailMessageBuilder SetDestinationAddresses(params string[] mailAddresses);
    }
}