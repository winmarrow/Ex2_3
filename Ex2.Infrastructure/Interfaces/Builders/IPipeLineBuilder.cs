using Ex2.Infrastructure.Abstractions;

namespace Ex2.Infrastructure.Interfaces
{
    /// <summary>
    /// Pipeline builder interface
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IPipeLineBuilder<TContext>
    {
        /// <summary>
        /// This method adds handler to pipeline handler's queue.
        /// </summary>
        /// <param name="handler">PipeLineHandler with same context type as builder.</param>
        /// <returns>Returns this builder.</returns>
        IPipeLineBuilder<TContext> UseHandler(IPipeLineHandler<TContext> handler);

        /// <summary>
        /// This method clears the queue of handlers used in the build process.
        /// </summary>
        IPipeLineBuilder<TContext> Clear();

        /// <summary>
        /// This method creates pipeline from handlers and clears handlers list.
        /// </summary>
        /// <returns>Returns the start point of the pipeline.</returns>
        PipeLineTask<TContext> Build();
    }
}