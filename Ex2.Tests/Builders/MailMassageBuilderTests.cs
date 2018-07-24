using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Ex2.BL.Entities.Builders;
using Ex2.BL.Entities.Validators;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Interfaces.Builders;
using NLog;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using static NUnit.StaticExpect.Expectations;

namespace Ex2.Tests.Builders
{
    public class MailMassageBuilderTests
    {
        private readonly ILogger _logger = Substitute.For<ILogger>();
        private IMailMessageBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder =  new MailMessageBuilder(_logger);
            _builder.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _logger.ClearSubstitute();
        }


        [Test]
        public void SetSenderAddress_Should_CatchArgumentNullException_And_WriteErrorToLog_When_SetNullAddress()
        {
            Expect(()=> _builder.SetSenderAddress(null), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetSenderAddress_Should_CatchArgumentException_And_WriteErrorToLog_When_SetEmptyAddress()
        {
            Expect(() => _builder.SetSenderAddress(String.Empty), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetSenderAddress_Should_CatchArgumentException_And_WriteErrorToLog_When_SetIncorrectAddress()
        {
            Expect(() => _builder.SetSenderAddress("someAddress"), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetSenderAddress_Should_DoNotThrowException_When_SetCorrectAddress()
        {
            Expect(() => _builder.SetSenderAddress("sender@test.t"), Throws.Nothing);
        }

        [Test]
        public void SetDestinationAddresses_Should_CatchArgumentNullException_And_WriteErrorToLog_When_SetNullAddress()
        {
            Expect(() => _builder.SetDestinationAddresses((string)null), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetDestinationAddresses_Should_TCatchArgumentNullException_And_WriteErrorToLog_When_SetNullAddresses()
        {
            Expect(() => _builder.SetDestinationAddresses(null, null), Throws.Nothing);
            _logger.Received(2).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetDestinationAddresses_Should_CatchArgumentException_And_WriteErrorToLog_When_SetEmptyAddress()
        {
            Expect(() => _builder.SetDestinationAddresses(String.Empty), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetDestinationAddresses_Should_CatchArgumentException_And_WriteErrorToLog_When_SetEmptyAddresses()
        {
            Expect(() => _builder.SetDestinationAddresses(String.Empty, String.Empty), Throws.Nothing);
            _logger.Received(2).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetDestinationAddresses_Should_CatchFormatException_And_WriteErrorToLog_When_SetIncorrectAddress()
        {
            Expect(() => _builder.SetDestinationAddresses("someAddress"), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetDestinationAddresses_Should_CatchFormatException_And_WriteErrorToLog_When_SetIncorrectAddresses()
        {
            Expect(() => _builder.SetDestinationAddresses("someAddress", "someOtherAddress"), Throws.Nothing);
            _logger.Received(2).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void SetDestinationAddresses_Should_DoNotThrowException_When_SetCorrectAddress()
        {
            Expect(() => _builder.SetDestinationAddresses("to@test.t"), Throws.Nothing);
            Expect(_builder.Build().To.Count, Is.EqualTo(1));
        }

        [Test]
        public void SetDestinationAddresses_Should_DoNotThrowException_When_SetCorrectAddresses()
        {
            Expect(() => _builder.SetDestinationAddresses("to1@test.t", "to2@test.t"), Throws.Nothing);
            Expect(_builder.Build().To.Count, Is.EqualTo(2));
        }

        [Test]
        public void SetSubject_Should_ThrowArgumentNullException_When_SetNullSubject()
        {
            Expect(() => _builder.SetSubject(null), Throws.TypeOf<ArgumentNullException>());
            
        }

        [Test]
        public void SetSubject_Should_DoNotThrowException_When_SetEmptySubject()
        {
            Expect(() => _builder.SetSubject(string.Empty), Throws.Nothing);
            Expect(_builder.Build().Subject, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SetSubject_Should_DoNotThrowException_When_SetAnySubject()
        {
            string subject = "someSubject";

            Expect(() => _builder.SetSubject(subject), Throws.Nothing);
            Expect(_builder.Build().Subject, Is.EqualTo(subject));
        }

        [Test]
        public void SetBody_Should_ThrowArgumentNullException_When_SetNullBody()
        {
            Expect(() => _builder.SetBody(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void SetBody_Should_DoNotThrowException_When_SetEmptySubject()
        {
            Expect(() => _builder.SetBody(string.Empty), Throws.Nothing);
            Expect(_builder.Build().Body, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SetBody_Should_DoNotThrowException_When_SetAnyBody()
        {
            string body = "someBody";

            Expect(() => _builder.SetBody(body), Throws.Nothing);
            Expect(_builder.Build().Body, Is.EqualTo(body));
        }

        [Test]
        public void AddAttachment_Should_ThrowArgumentNullException_When_SetNullStream()
        {
            Expect(() => _builder.AddAttachment(null, String.Empty), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void AddAttachment_Should_DoNotThrowException_When_SetStreamWhichCanBeRead()
        {
            using (var stream = new MemoryStream())
            {
                Expect(() => _builder.AddAttachment(stream, String.Empty), Throws.Nothing);
                Expect(_builder.Build().Attachments.Count, Is.EqualTo(1));
            }
            
        }
    }
}