using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Ex2.BL.Entities.Builders;
using Ex2.BL.Entities.PipeLineHandlers;
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
    public class FileDeleteHandlerTests
    {
        private const int RetryCount = 3;

        private readonly ILogger _logger = Substitute.For<ILogger>();
        private readonly IFileInfo _fileInfo = Substitute.For<IFileInfo>();

        private IPipeLineHandler<IFileInfo> _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new FileDeleteHandler(_logger);
        }

        [TearDown]
        public void TearDown()
        {
            _logger.ClearSubstitute();
            _fileInfo.ClearSubstitute();
        }

        [Test]
        public void ExecuteAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            Expect(()=> _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.TypeOf<TaskCanceledException>());
        }

        [Test]
        public void ExecuteAsync_Should_CatchFileNotFoundException_And_WriteErrorToLog_When_FileDoNotExist()
        {
            var cts = new CancellationTokenSource();
            _fileInfo.Exists.Returns(false);
            
            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void ExecuteAsync_Should_CatchIOException_And_WriteErrorToLog_When_FileCannotBeOpened()
        {
            var cts = new CancellationTokenSource();
            _fileInfo.Exists.Returns(true);
            _fileInfo
                .When(x => x.Delete())
                .Do(x => { throw new IOException(); });

            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.Nothing);
            _logger.Received(RetryCount).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void ExecuteAsync_Should_CatchUnauthorizedAccessException_And_WriteErrorToLog_When_DoNotHaveAccessToFile()
        {
            var cts = new CancellationTokenSource();
            _fileInfo.Exists.Returns(true);
            _fileInfo
                .When(x => x.Delete())
                .Do(x => { throw new UnauthorizedAccessException(); });

            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void ExecuteAsync_Should_CatchSecurityException_And_WriteErrorToLog_When_SecurityProblems()
        {
            var cts = new CancellationTokenSource();
            _fileInfo.Exists.Returns(true);
            _fileInfo
                .When(x => x.Delete())
                .Do(x => { throw new SecurityException(); });

            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.Nothing);
            _logger.Received(1).Error(Arg.Any<Exception>(), Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void ExecuteAsync_Should_DoNotThrowException_And_WriteInfoToLog_When_NoProblems()
        {
            var cts = new CancellationTokenSource();
            _fileInfo.Exists.Returns(true);

            Expect(() => _handler.ExecuteAsync(_fileInfo, cts.Token), Throws.Nothing);
            _logger.Received(1).Info(Arg.Any<string>(), Arg.Any<object[]>());
        }
    }
}
