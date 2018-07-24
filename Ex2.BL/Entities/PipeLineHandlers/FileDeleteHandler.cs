using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.Infrastructure.Interfaces.Wrappers;
using NLog;

namespace Ex2.BL.Entities.PipeLineHandlers
{
    [Inject(typeof(TraceAspect))]
    public class FileDeleteHandler : BaseFileInfoHandler
    {
        private const string ExceptionMsg = @"File ""{0}"" wasn't deleted, Reason: {1}.";
        private const string NotExistMsg = @"File ""{0}"" wasn't deleted, Reason: file don't exist.";
        private const string SuccessMsg = @"File ""{0}"" was deleted.";

        public FileDeleteHandler(ILogger logger)
            : base(logger)
        {
        }

        public int MaxRetryCount { get; set; } = 3;

        public override async Task ExecuteAsync(IFileInfo fileInfo, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            await Next(fileInfo, token).ConfigureAwait(false);

            DeleteWithRepeat(fileInfo);
        }

        private void DeleteWithRepeat(IFileInfo fileInfo, int retryNumber = 0)
        {
            try
            {
                Delete(fileInfo);
            }
            catch (IOException e)
            {
                LogError(e, ExceptionMsg, fileInfo.Name, e.GetType().Name);

                if (retryNumber < MaxRetryCount - 1)
                {
                    DeleteWithRepeat(fileInfo, retryNumber + 1);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                LogError(e, ExceptionMsg, fileInfo.Name, e.GetType().Name);
            }
            catch (SecurityException e)
            {
                LogError(e, ExceptionMsg, fileInfo.Name, e.GetType().Name);
            }
        }

        private void Delete(IFileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                fileInfo.Delete();

                LogInfo(SuccessMsg, fileInfo.FullName);
            }
            else
            {
                LogError(new FileNotFoundException(), NotExistMsg, fileInfo.FullName);
            }
        }
    }
}