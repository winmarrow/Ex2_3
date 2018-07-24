using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Builders;
using Ex2.Infrastructure.Interfaces.Wrappers;
using NLog;

namespace Ex2.BL.Entities.PipeLineHandlers
{
    [Inject(typeof(TraceAspect))]
    public class FileSendHandler : BaseFileInfoHandler
    {
        private const string MailExceptionMsg = @"File ""{0}"" wasn't sent, Reason: {1}";
        private const string ClientExceptionMsg = @"File ""{0}"" wasn't sent, Reason: {1}";
        private const string SuccessMsg = @"File ""{0}"" was sent.";
        private readonly IMailMessageBuilder _messageBuilder;
        private readonly IMailSender _sender;

        private readonly AppSettings _settings;

        public FileSendHandler(AppSettings settings, IMailSender sender, IMailMessageBuilder messageBuilder, ILogger logger)
            : base(logger)
        {
            _settings = settings;
            _sender = sender;
            _messageBuilder = messageBuilder;
        }

        public override Task ExecuteAsync(IFileInfo fileInfo, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            SendFileByMailMessage(fileInfo);

            return Next(fileInfo, token);
        }

        private void SendFileByMailMessage(IFileInfo fileInfo)
        {
            MailMessage mailMessage = null;
            try
            {
                mailMessage = CreateMessage(fileInfo);
                _sender.Send(mailMessage);

                LogInfo(SuccessMsg, fileInfo.FullName);
            }
            catch (InvalidMailExeption e)
            {
                LogError(e, MailExceptionMsg, fileInfo.Name, e.GetType().Name);
            }
            catch (SmptClientException e)
            {
                LogError(e, ClientExceptionMsg, fileInfo.Name, e.GetType().Name);
            }
            finally
            {
                mailMessage?.Dispose();
            }
        }

        private MailMessage CreateMessage(IFileInfo fileInfo)
        {
            return _messageBuilder
                .Create()
                .SetSenderAddress(_settings.Mail.SenderEmailAddres)
                .SetDestinationAddresses(_settings.Mail.ToEmailAddreses)
                .SetSubject(_settings.Mail.Subject)
                .SetBody(_settings.Mail.Body)
                .AddAttachment(fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read), fileInfo.Name)
                .Build();
        }
    }
}