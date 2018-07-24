using System;
using System.Net;
using System.Net.Mail;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Factories;
using Ex2.Infrastructure.Interfaces.Validators;
using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.BL.Entities
{
    [Inject(typeof(TraceAspect))]
    public class MailSender : IMailSender
    {
        private readonly ISmtpClientFactory _factory;
        private readonly AppSettings _settings;
        private readonly IMailMessageValidator _validator;

        public MailSender(AppSettings settings, ISmtpClientFactory factory, IMailMessageValidator validator)
        {
            _settings = settings;
            _factory = factory;
            _validator = validator;
        }

        public void Send(MailMessage mailMessage)
        {
            _validator.Validate(mailMessage);

            ISmtpClient client = null;
            try
            {
                client = _factory.Create();
                SetUpClient(client);

                client.Send(mailMessage);
            }
            catch (ArgumentException e)
            {
                throw new SmptClientException("Cannot setup client.", e);
            }
            catch (InvalidOperationException e)
            {
                throw new SmptClientException("Client cannot do operation.", e);
            }
            catch (SmtpException e)
            {
                throw new SmptClientException("Client cannot send mail message.", e);
            }
            finally
            {
                client?.Dispose();
            }
        }

        private void SetUpClient(ISmtpClient client)
        {
            var credential = new NetworkCredential(
                _settings.Client.CredentialMailAddress,
                _settings.Client.CredentialMailPassword);

            client.Host = _settings.Client.SmptHost;
            client.Port = _settings.Client.SmptPort;
            client.Timeout = _settings.Client.SendTimeOut;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = credential;
            client.EnableSsl = true;
        }
    }
}