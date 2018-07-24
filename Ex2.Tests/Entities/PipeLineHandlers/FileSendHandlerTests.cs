using System;
using System.IO;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Ex2.BL;
using Ex2.BL.Entities.Builders;
using Ex2.BL.Entities.PipeLineHandlers;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Builders;
using Ex2.Infrastructure.Interfaces.Wrappers;
using NLog;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using NUnit.Framework;
using static NUnit.StaticExpect.Expectations;

namespace Ex2.Tests.Entities.PipeLineHandlers
{
    [TestFixture]
    public class FileSendHandlerTests
    {
        private const int RetryCount = 3;

        private readonly ILogger _logger = Substitute.For<ILogger>();
        private readonly AppSettings _settings = Substitute.For<AppSettings>();
        private readonly IMailSender _mailSender = Substitute.For<IMailSender>();
        private readonly IMailMessageBuilder _messageBuilder = Substitute.For<IMailMessageBuilder>();

        private readonly IFileInfo _fileInfo = Substitute.For<IFileInfo>();

        private IPipeLineHandler<IFileInfo> _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new FileSendHandler(_settings, _mailSender, _messageBuilder, _logger);
        }

        [TearDown]
        public void TearDown()
        {
            _logger.ClearSubstitute();
            _settings.ClearSubstitute();
            _mailSender.ClearSubstitute();
            _messageBuilder.ClearSubstitute();

            _fileInfo.ClearSubstitute();
        }

        [Test]
        public void ExecuteAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.TypeOf<OperationCanceledException>());
        }

        [Test]
        public void ExecuteAsync_Should_CatchInvalidMailExeption_And_WriteErrorToLog_When_BuiltMessageIsInvalid()
        {
            var cts = new CancellationTokenSource();
            _mailSender
                .When(x => x.Send(Arg.Any<MailMessage>()))
                .Do(x => { throw new SmptClientException(); });

            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void ExecuteAsync_Should_CatchSmptClientException_And_WriteErrorToLog_When_SmtpClientCannotSend()
        {
            var cts = new CancellationTokenSource();
            _mailSender
                .When(x => x.Send(Arg.Any<MailMessage>()))
                .Do(x => { throw new SmptClientException(); });

            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void ExecuteAsync_Should_DoNotThrowExeption_And_WriteErrorToLog_When_MessageIsValidAndSmptClientSentMessage()
        {
            var cts = new CancellationTokenSource();
            
            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.Nothing);
            _logger.Received(1).Info(Arg.Any<string>(), Arg.Any<object[]>());
        }
    }
}
