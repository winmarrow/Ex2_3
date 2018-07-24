using System.Text;
using AspectInjector.Broker;
using NLog;
using static AspectInjector.Broker.Advice;
using static AspectInjector.Broker.Advice.Argument;
using static AspectInjector.Broker.Advice.Target;
using static AspectInjector.Broker.Advice.Type;
using static AspectInjector.Broker.Aspect;
using Type = System.Type;

namespace Ex2.BL.Aspects
{
    [Aspect(Scope.Global)]
    public class TraceAspect
    {
        private readonly ILogger _logger;

        public TraceAspect()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        [Advice(Before, Constructor)]
        public void TraceConstructorBefore([Argument(Source.Type)] Type type, [Argument(Source.Arguments)] object[] args)
        {
            var messageFormat = GetStringFormatWithArgs($"[{type.Name}] Begin of constructor", args.Length);
            LogToTrace(messageFormat, args);
        }

        [Advice(After, Constructor)]
        public void TraceConstructorAfter([Argument(Source.Type)] Type type)
        {
            LogToTrace($"[{type.Name}] End of constructor.");
        }

        [Advice(Before, Method)]
        public void TraceMethodBefore([Argument(Source.Type)] Type type, [Argument(Source.Name)] string name, [Argument(Source.Arguments)] object[] args)
        {
            var messageFormat = GetStringFormatWithArgs($"[{type.Name}] Input to method \"{name}\"", args.Length);
            LogToTrace(messageFormat, args);
        }

        [Advice(After, Method)]
        public void TraceMethodAfter([Argument(Source.Type)] Type type, [Argument(Source.Name)] string name)
        {
            LogToTrace($"[{type.Name}] End of method \"{name}\".");
        }

        private void LogToTrace(string msgFormat, params object[] args)
        {
            _logger?.Trace(msgFormat, args);
        }

        private string GetStringFormatWithArgs(string startOfString, int argsCount)
        {
            var builder = new StringBuilder(startOfString);

            if (argsCount > 0)
            {
                builder.Append(argsCount == 1 ? " with argument:" : " with arguments:");

                for (var i = 0; i < argsCount; i++)
                {
                    builder.Append($" {{{i.ToString()}}}");
                }
            }
            else
            {
                builder.Append(" without arguments");
            }

            builder.Append('.');

            return builder.ToString();
        }
    }
}