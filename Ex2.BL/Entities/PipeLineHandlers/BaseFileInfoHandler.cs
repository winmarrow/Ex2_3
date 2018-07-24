using System;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces.Wrappers;
using NLog;

namespace Ex2.BL.Entities.PipeLineHandlers
{
    public abstract class BaseFileInfoHandler : PipeLineHandler<IFileInfo>
    {
        public BaseFileInfoHandler(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        protected void LogInfo(string infoMessage, params object[] args)
        {
            Logger?.Info(infoMessage, args);
        }

        protected void LogError(Exception exception, string errorMessage, params object[] args)
        {
            Logger?.Error(exception, errorMessage, args);
        }
    }
}