using System;
using System.Collections.Generic;
using System.Linq;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces;

namespace Ex2.BL.Entities.Builders
{
    /// <summary>
    ///     Pipeline builder implementation class
    /// </summary>
    /// <typeparam name="TContext">PipeLine context</typeparam>
    [Inject(typeof(TraceAspect))]
    public sealed class PipeLineBuilder<TContext> : IPipeLineBuilder<TContext>
    {
        private readonly IList<IPipeLineHandler<TContext>> _handlers = new List<IPipeLineHandler<TContext>>();

        public IPipeLineBuilder<TContext> UseHandler(IPipeLineHandler<TContext> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");
            }

            if (_handlers.Contains(handler))
            {
                throw new ArgumentException("This handler already using", nameof(handler));
            }

            _handlers.Add(handler);

            return this;
        }

        public IPipeLineBuilder<TContext> Clear()
        {
            _handlers.Clear();

            return this;
        }

        public PipeLineTask<TContext> Build()
        {
            if (!_handlers.Any())
            {
                throw new EmptyCollectionException("Need one or more handlers");
            }

            var handler = _handlers[0];
            PipeLineTask<TContext> pieLineTask = handler.ExecuteAsync;

            foreach (var nextHandler in _handlers.Skip(1))
            {
                handler.SetNext(nextHandler);
                handler = nextHandler;
            }

            Clear();

            return pieLineTask;
        }
    }
}