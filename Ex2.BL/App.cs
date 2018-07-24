using System;
using System.Threading;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.BL.Entities.PipeLineHandlers;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Wrappers;
using Ex3.DI;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Ex2.BL
{
    [Inject(typeof(TraceAspect))]
    public class App : IDisposable
    {
        private readonly CriticalAndCancelExceptionHandler _criticalAndCancelExceptionHandler;
        private readonly FileDeleteHandler _fileDaleteHandler;
        private readonly FileSendHandler _fileSendHandler;
        private readonly IPipeLineBuilder<IFileInfo> _pipeLineBuilder;
        private readonly IDirectoryWatcher _watcher;

        private bool _disposedValue;
        private PipeLineTask<IFileInfo> _pipeLine;
        private CancellationTokenSource _cancellationTokenSource;

        public App(IDirectoryWatcher watcher, IPipeLineBuilder<IFileInfo> pipeLineBuilder, CriticalAndCancelExceptionHandler criticalAndCancelExceptionHandler, FileDeleteHandler fileDaleteHandler, FileSendHandler fileSendHandler)
        {
            _watcher = watcher;
            _pipeLineBuilder = pipeLineBuilder;
            _criticalAndCancelExceptionHandler = criticalAndCancelExceptionHandler;
            _fileDaleteHandler = fileDaleteHandler;
            _fileSendHandler = fileSendHandler;
        }

        ~App()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Init()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            ConfigureLogging();
            ConfigurePipeLine();
            ConfigureWatcher();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Cancel();
                }

                _cancellationTokenSource.Dispose();
                _watcher.Dispose();

                _disposedValue = true;
            }
        }

        private void ConfigureWatcher()
        {
            _watcher.Init();
            _watcher.Changed += (sender, info) => _pipeLine(info, _cancellationTokenSource.Token).ConfigureAwait(false);
        }

        private void ConfigureLogging()
        {
            var config = new LoggingConfiguration();

            ConfigureFileTarget(config);
            ConfigureConsoleTarget(config);

            LogManager.Configuration = config;
        }

        private void ConfigureConsoleTarget(LoggingConfiguration config)
        {
            var consoleTarget = new ColoredConsoleTarget("consoleTarget")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception:format=toString}"
            };
            config.AddTarget(consoleTarget);

            var loglevel = LogLevel.Off;
            switch (DiContainer.Instance.GetInstance<AppSettings>().LogLevel)
            {
                case 0:
                    loglevel = LogLevel.Fatal;
                    break;
                case 1:
                    loglevel = LogLevel.Error;
                    break;
                case 2:
                    loglevel = LogLevel.Warn;
                    break;
                case 3:
                    loglevel = LogLevel.Info;
                    break;
                case 4:
                    loglevel = LogLevel.Debug;
                    break;
                case 5:
                    loglevel = LogLevel.Trace;
                    break;
                default:
                    loglevel = LogLevel.Trace;
                    break;
            }

            config.AddRule(loglevel, LogLevel.Fatal, consoleTarget);
        }

        private void ConfigureFileTarget(LoggingConfiguration config)
        {
            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = "${basedir}/log.txt",
                Layout = "${longdate} ${level} ${message} ${exception:format=toString}"
            };
            config.AddTarget(fileTarget);
            config.AddRuleForAllLevels(fileTarget);
        }

        private void ConfigurePipeLine()
        {
            _pipeLine = _pipeLineBuilder
                .UseHandler(_criticalAndCancelExceptionHandler)
                .UseHandler(_fileDaleteHandler)
                .UseHandler(_fileSendHandler)
                .Build();
        }
    }
}