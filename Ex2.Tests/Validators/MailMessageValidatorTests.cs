using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Ex2.BL.Entities.Validators;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Validators;
using NSubstitute;
using NUnit.Framework;
using static NUnit.StaticExpect.Expectations;

namespace Ex2.Tests.Validators
{
    public class MailMessageValidatorTests
    {
        private IMailMessageValidator _validator;
        private MailMessage _mailMessage;

        [SetUp]
        public void SetUp()
        {
            _validator =  new MailMessageValidator();
            _mailMessage = new MailMessage();
        }

        [TearDown]
        public void TearDown()
        {
            _mailMessage?.Dispose();
        }


        [Test]
        public void ValidateSender_Should_ThrowMailBuildExeption_When_SenderWasNotSet()
        {
            Expect(()=> _validator.ValidateSenderAddress(_mailMessage), Throws.TypeOf<InvalidMailExeption>());
        }

        [Test]
        public void ValidateDestinationAddress_Should_ThrowMailBuildExeption_When_DestinationAddressWasNotSet()
        {
            Expect(() => _validator.ValidateDestinationAddress(_mailMessage), Throws.TypeOf<InvalidMailExeption>());
        }
        
        [Test]
        public void ValidateSubject_Should_ThrowMailBuildExeption_When_SubjectWasNotSet()
        {
            Expect(() => _validator.ValidateSubject(_mailMessage), Throws.TypeOf<InvalidMailExeption>());
        }

        [Test]
        public void ValidateBody_Should_ThrowMailBuildExeption_When_BodyWasNotSet()
        {
            Expect(() => _validator.ValidateBody(_mailMessage), Throws.TypeOf<InvalidMailExeption>());
        }

        [Test]
        public void ValidateAttachments_Should_ThrowMailBuildExeption_When_AttachmentsWereNotAdded()
        {
            Expect(() => _validator.ValidateAttachments(_mailMessage), Throws.TypeOf<InvalidMailExeption>());
        }

        [Test]
        public void Validate_Should_ThrowMailBuildExeption_When_NothingWasSet()
        {
            Expect(() => _validator.Validate(_mailMessage), Throws.TypeOf<InvalidMailExeption>());
        }

        [Test]
        public void Validate_Should_DoNotThrowMailBuildExeption()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine("TestLine");
                }

                _mailMessage.Sender = new MailAddress("sender@test");
                _mailMessage.From = _mailMessage.Sender;

                _mailMessage.To.Add(new MailAddress("to@test"));
                _mailMessage.Subject = "Test subject";
                _mailMessage.Body = "Test body";
                _mailMessage.Attachments.Add(new Attachment(stream,null,null));
            }

            Expect(() => _validator.Validate(_mailMessage), Throws.Nothing);
        }
    }
}