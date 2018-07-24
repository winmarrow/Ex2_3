using System;
using System.Net.Mail;
using Ex2.BL;
using Ex2.BL.Entities;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Factories;
using Ex2.Infrastructure.Interfaces.Validators;
using Ex2.Infrastructure.Interfaces.Wrappers;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using static NUnit.StaticExpect.Expectations;

namespace Ex2.Tests.Entities
{
    [TestFixture]
    public class MailSenderTests
    {
        private readonly AppSettings _settings = Substitute.For<AppSettings>();
        private readonly ISmtpClientFactory _clientFactory = Substitute.For<ISmtpClientFactory>();
        private readonly IMailMessageValidator _mailMessageValidator = Substitute.For<IMailMessageValidator>();

        private IMailSender _mailSender;

        [SetUp]
        public void SetUp()
        {
            _mailSender = new MailSender(_settings, _clientFactory, _mailMessageValidator);
        }

        [TearDown]
        public void TearDown()
        {

            _settings.ClearSubstitute();
            _clientFactory.ClearSubstitute();
            _mailMessageValidator.ClearSubstitute();
        }

        [Test]
        public void Send_Should_ThrowSmptClientException_When_ClientThrowArgumentException()
        {
            var client = Substitute.For<ISmtpClient>();
            client.When(c => c.Send(Arg.Any<MailMessage>())).Do(info => throw new ArgumentException());

            _clientFactory.Create().Returns(client);

            Expect(() => _mailSender.Send(Arg.Any<MailMessage>()), Throws.TypeOf<SmptClientException>());
        }

        [Test]
        public void Send_Should_ThrowSmptClientException_When_ClientThrowInvalidOperationException()
        {
            var client = Substitute.For<ISmtpClient>();
            client.When(c => c.Send(Arg.Any<MailMessage>())).Do(info => throw new InvalidOperationException());

            _clientFactory.Create().Returns(client);

            Expect(() => _mailSender.Send(Arg.Any<MailMessage>()), Throws.TypeOf<SmptClientException>());
        }

        [Test]
        public void Send_Should_ThrowSmptClientException_When_ClientThrowSmtpException()
        {
            var client = Substitute.For<ISmtpClient>();
            client.When(c => c.Send(Arg.Any<MailMessage>())).Do(info => throw new SmtpException());

            _clientFactory.Create().Returns(client);

            Expect(() => _mailSender.Send(Arg.Any<MailMessage>()), Throws.TypeOf<SmptClientException>());
        }
    }
}
