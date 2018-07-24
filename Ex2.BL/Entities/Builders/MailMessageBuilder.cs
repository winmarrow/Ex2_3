using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces.Builders;
using NLog;

namespace Ex2.BL.Entities.Builders
{
    [Inject(typeof(TraceAspect))]
    public class MailMessageBuilder : Builder<MailMessage>, IMailMessageBuilder
    {
        private readonly ILogger _logger;

        private MailMessage _mailMessage;

        public MailMessageBuilder(ILogger logger)
        {
            _logger = logger;
        }

        public IMailMessageBuilder Create()
        {
            _mailMessage?.Dispose();
            _mailMessage = new MailMessage();

            return this;
        }

        public IMailMessageBuilder SetSenderAddress(string mailAddress)
        {
            var address = CreateMailAddress(mailAddress);

            if (address != null)
            {
                _mailMessage.From = address;
                _mailMessage.Sender = address;
            }

            return this;
        }

        public IMailMessageBuilder SetDestinationAddresses(params string[] mailAddresses)
        {
            if (mailAddresses == null)
            {
                throw new ArgumentNullException(nameof(mailAddresses));
            }

            foreach (var mailAddress in mailAddresses)
            {
                var address = CreateMailAddress(mailAddress);
                if (address != null)
                {
                    _mailMessage.To.Add(address);
                }
            }

            return this;
        }

        public IMailMessageBuilder SetSubject(string subject)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            _mailMessage.SubjectEncoding = Encoding.UTF8;
            _mailMessage.Subject = subject;
            return this;
        }

        public IMailMessageBuilder SetBody(string body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            _mailMessage.BodyEncoding = Encoding.UTF8;
            _mailMessage.Body = body;
            return this;
        }

        public IMailMessageBuilder AddAttachment(Stream stream, string fileName)
        {
            var attachment = new Attachment(stream, fileName, MediaTypeNames.Application.Octet);

            _mailMessage.Attachments.Add(attachment);
            return this;
        }

        public override MailMessage Build()
        {
            var message = _mailMessage;
            _mailMessage = null;
            return message;
        }

        private MailAddress CreateMailAddress(string mailAddress)
        {
            MailAddress mail = null;
            try
            {
                mail = new MailAddress(mailAddress);
            }
            catch (FormatException e)
            {
                _logger.Error(e, "Incorrect mail address format: {0}", mailAddress);
            }
            catch (ArgumentNullException e)
            {
                _logger.Error(e, "Incorrect mail address format: {0}", mailAddress);
            }
            catch (ArgumentException e)
            {
                _logger.Error(e, "Incorrect mail address format: {0}", mailAddress);
            }

            return mail;
        }
    }
}