using System;
using System.Threading;
using System.Threading.Tasks;
using Ex2.Infrastructure.Interfaces;

namespace Ex2.Infrastructure.Abstractions
{

    /// <summary>
    /// Delegate which encapsulates pipeline handler task
    /// </summary>
    /// <typeparam name="TContext">Context type</typeparam>
    /// <param name="context">context</param>
    /// <param name="token">Cancellation token</param>
    /// <returns></returns>
    public delegate Task PipeLineTask<in TContext>(TContext context, CancellationToken token);

    /// <summary>
    /// Abstract pipeline handler class
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class PipeLineHandler<TContext> : IPipeLineHandler<TContext>
    {
        /// <summary>
        /// Contains next pipeline handler method if it was set or end task
        /// </summary>
        protected PipeLineTask<TContext> Next { get; set; } = (context, token) => Task.CompletedTask;

        public void SetNext(IPipeLineHandler<TContext> nextHandler)
        {
            if (nextHandler == null)
                throw new ArgumentNullException(nameof(nextHandler), "Next handler cannot be null");

            if (nextHandler == this)
                throw new ArgumentException("Next handler cannot be same handler", nameof(nextHandler));

            Next = nextHandler.ExecuteAsync;
        }

        public abstract Task ExecuteAsync(TContext context, CancellationToken token);
    }

    
}