using System;
using System.IO;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Ex2.BL;
using Ex2.BL.Entities;
using Ex2.BL.Entities.Builders;
using Ex2.BL.Entities.PipeLineHandlers;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Builders;
using Ex2.Infrastructure.Interfaces.Factories;
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
    public class DirectoryWatcherTests
    {
        private readonly ILogger _logger = Substitute.For<ILogger>();
        private readonly AppSettings _settings = Substitute.For<AppSettings>();
        private readonly IFileSystemWatcherFactory _fileSystemWatcherFactory = Substitute.For<IFileSystemWatcherFactory>();
        private readonly IFileInfoFactory _fileInfoFactory = Substitute.For<IFileInfoFactory>();

        private IDirectoryWatcher _directoryWatcher;

        [SetUp]
        public void SetUp()
        {
            _directoryWatcher = new DirectoryWatcher(_settings, _fileSystemWatcherFactory, _fileInfoFactory);
        }

        [TearDown]
        public void TearDown()
        {
            _logger.ClearSubstitute();
            _settings.ClearSubstitute();
            _fileSystemWatcherFactory.ClearSubstitute();
            _fileInfoFactory.ClearSubstitute();
        }

        [Test]
        public void OnChanged_Should_NotRaiseChangedEvent_When_NewFileCreatedAlreadyDoNotExist()
        {
            bool IsRiced = false;

            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Exists.Returns(false);
            _fileInfoFactory.Create(Arg.Any<string>()).Returns(fileInfo);

            var watcher = Substitute.For<IFileSystemWatcher>();
            _fileSystemWatcherFactory.Create().Returns(watcher);

            _directoryWatcher.Changed += (sender, info) => IsRiced = true;

            _directoryWatcher.Init();
            watcher.Created += Raise.Event<FileSystemEventHandler>(watcher, new FileSystemEventArgs(WatcherChangeTypes.Created, String.Empty, "fileName"));

            Expect(IsRiced, Is.False);
        }


        [Test]
        public void OnChanged_Should_NotRaiseChangedEvent_When_NewFileCreatedWithLengthEqualZero()
        {
            bool IsRiced = false;

            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Exists.Returns(true);
            fileInfo.Length.Returns(0);
            _fileInfoFactory.Create(Arg.Any<string>()).Returns(fileInfo);

            var watcher = Substitute.For<IFileSystemWatcher>();
            _fileSystemWatcherFactory.Create().Returns(watcher);

            _directoryWatcher.Changed += (sender, info) => IsRiced = true;

            _directoryWatcher.Init();
            watcher.Created += Raise.Event<FileSystemEventHandler>(watcher, new FileSystemEventArgs(WatcherChangeTypes.Created, String.Empty, "fileName"));

            Expect(IsRiced, Is.False);
        }

        [Test]
        public void OnChanged_Should_RaiseChangedEvent_When_NewFileCreatedWithLengthMoreThenZero()
        {
            bool IsRiced = false;

            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Exists.Returns(true);
            fileInfo.Length.Returns(1);

            _fileInfoFactory.Create(Arg.Any<string>()).Returns(fileInfo);

            var watcher = Substitute.For<IFileSystemWatcher>();


            _fileSystemWatcherFactory.Create().Returns(watcher);

            _directoryWatcher.Changed += (sender, info) => IsRiced = true;

            _directoryWatcher.Init();
            watcher.Created += Raise.Event<FileSystemEventHandler>(watcher, new FileSystemEventArgs(WatcherChangeTypes.Created, String.Empty, "fileName"));

            Expect(IsRiced, Is.True);
        }

        [Test]
        public void OnChanged_Should_NotRaiseChangedEvent_When_FileChanged_And_ItLengthEqualZero()
        {
            bool IsRiced = false;

            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Exists.Returns(true);
            fileInfo.Length.Returns(0);
            _fileInfoFactory.Create(Arg.Any<string>()).Returns(fileInfo);

            var watcher = Substitute.For<IFileSystemWatcher>();
            _fileSystemWatcherFactory.Create().Returns(watcher);

            _directoryWatcher.Changed += (sender, info) => IsRiced = true;

            _directoryWatcher.Init();
            watcher.Changed += Raise.Event<FileSystemEventHandler>(watcher, new FileSystemEventArgs(WatcherChangeTypes.Created, String.Empty, "fileName"));

            Expect(IsRiced, Is.False);
        }

        [Test]
        public void OnChanged_Should_RaiseChangedEvent_When_FileChanged_And_ItLengthMoreThenZero()
        {
            bool IsRiced = false;

            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Exists.Returns(true);
            fileInfo.Length.Returns(1);
            _fileInfoFactory.Create(Arg.Any<string>()).Returns(fileInfo);

            var watcher = Substitute.For<IFileSystemWatcher>();
            _fileSystemWatcherFactory.Create().Returns(watcher);

            _directoryWatcher.Changed += (sender, info) => IsRiced = true;

            _directoryWatcher.Init();
            watcher.Changed += Raise.Event<FileSystemEventHandler>(watcher, new FileSystemEventArgs(WatcherChangeTypes.Created, String.Empty, "fileName"));

            Expect(IsRiced, Is.True);
        }
    }
}
