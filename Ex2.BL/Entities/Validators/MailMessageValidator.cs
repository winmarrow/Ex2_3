using System.Linq;
using System.Net.Mail;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces.Validators;

namespace Ex2.BL.Entities.Validators
{
    [Inject(typeof(TraceAspect))]
    public class MailMessageValidator : Validator<MailMessage>, IMailMessageValidator
    {
        public void ValidateSenderAddress(MailMessage mailMessage)
        {
            if (mailMessage.From == null || mailMessage.Sender == null)
            {
                throw new InvalidMailExeption("The sender address wasn't set");
            }
        }

        public void ValidateDestinationAddress(MailMessage mailMessage)
        {
            if (!mailMessage.To.Any())
            {
                throw new InvalidMailExeption("No one destination address was set.");
            }
        }

        public void ValidateSubject(MailMessage mailMessage)
        {
            if (string.IsNullOrWhiteSpace(mailMessage.Subject))
            {
                throw new InvalidMailExeption("The mail subject wasn't set.");
            }
        }

        public void ValidateBody(MailMessage mailMessage)
        {
            if (string.IsNullOrWhiteSpace(mailMessage.Body))
            {
                throw new InvalidMailExeption("The mail body wasn't set.");
            }
        }

        public void ValidateAttachments(MailMessage mailMessage)
        {
            if (!mailMessage.Attachments.Any())
            {
                throw new InvalidMailExeption("No one attachment was added.");
            }
        }

        public override void Validate(MailMessage mailMessage)
        {
            ValidateIsNotNull(mailMessage);

            ValidateSenderAddress(mailMessage);

            ValidateDestinationAddress(mailMessage);

            ValidateSubject(mailMessage);

            ValidateBody(mailMessage);

            ValidateAttachments(mailMessage);
        }
    }
}