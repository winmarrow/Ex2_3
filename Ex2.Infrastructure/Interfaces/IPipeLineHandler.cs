using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ex2.Infrastructure.Interfaces
{

    /// <summary>
    /// PipeLine handler interface
    /// </summary>
    /// <typeparam name="TContext">Context type</typeparam>
    public interface IPipeLineHandler<TContext>
    {
        /// <summary>
        /// This method will set the following handler.
        /// </summary>
        /// <param name="nextHandler">Next handler</param>
        /// <exception cref="ArgumentNullException">Next handler cannot be null.</exception>
        /// <exception cref="ArgumentException">Next handler cannot be same handler.</exception>
        void SetNext(IPipeLineHandler<TContext> nextHandler);

        /// <summary>
        /// This method contains handler logic.
        /// </summary>
        /// <param name="context">Context that will pass through handlers.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task</returns>
        Task ExecuteAsync(TContext context, CancellationToken token);
    }
}