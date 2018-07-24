using System.Net.Mail;

namespace Ex2.Infrastructure.Interfaces.Validators
{
    public interface IMailMessageValidator : IValidator<MailMessage>
    {
        void ValidateAttachments(MailMessage mailMessage);

        void ValidateBody(MailMessage mailMessage);

        void ValidateDestinationAddress(MailMessage mailMessage);

        void ValidateSenderAddress(MailMessage mailMessage);

        void ValidateSubject(MailMessage mailMessage);
    }
}