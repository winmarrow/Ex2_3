using System;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.Infrastructure.Interfaces.Wrappers;
using NLog;

namespace Ex2.BL.Entities.PipeLineHandlers
{
    [Inject(typeof(TraceAspect))]
    public class CriticalAndCancelExceptionHandler : BaseFileInfoHandler
    {
        private const string UnhandledEceptionMsg = "Unhandled exception, filename: {0}";
        private const string OperationCanceledMsg = "Operation was canceled, filename: {0}";

        public CriticalAndCancelExceptionHandler(ILogger logger)
            : base(logger)
        {
        }

        public override async Task ExecuteAsync(IFileInfo fileInfo, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                await Next(fileInfo, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                LogInfo(OperationCanceledMsg, fileInfo.Name);
            }
            catch (Exception exception)
            {
                LogError(exception, UnhandledEceptionMsg, fileInfo.Name);
            }
        }
    }
}