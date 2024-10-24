using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace LoggingBenchmark
{
    public static class MemoizeLoggingExtensions
    {
        // TODO add memo container for different number of generic arguments 
        private static class MemoContainer<T1, T2, T3>
        {
            public static readonly ConcurrentDictionary<string, Action<ILogger, T1, T2, T3, Exception?>> Warnings = new();
            // TODO add memo container for different log levels. 
            // This will allow us to use only the string message as a dictionary key 
            // instead of dealing with a composite key of logLevel and message 
            // and improves dictionary access performance 

            public static readonly ConcurrentDictionary<int, Action<ILogger, T1, T2, T3, Exception?>> HashWarnings = new();

        }

        public static void MemoizeLogWarning<T1, T2, T3>(
            this ILogger logger,
            string messageTemplate,
            T1 param1,
            T2 param2,
            T3 param3,
            Exception? exception = null
        )
        {
            //var exist = MemoContainer<T1, T2, T3>.Warning.ContainsKey(message);

            var log = MemoContainer<T1, T2, T3>.Warnings.GetOrAdd(
                messageTemplate,
                _ => LoggerMessage.Define<T1, T2, T3>(
                    LogLevel.Warning,
                    eventId: 0,
                    formatString: messageTemplate
                )
            );

            //exception ??= new Exception(messageTemplate);
            log(logger, param1, param2, param3, exception);
        }

        public static void HashMemoizeLogWarning<T1,T2,T3>(
            this ILogger logger,
            string messageTemplate,
            T1 param1,
            T2 param2,
            T3 param3,
            Exception? exception = null)
        {
            var log = MemoContainer<T1, T2, T3>.HashWarnings.GetOrAdd(
                messageTemplate.GetHashCode(),
                _ => LoggerMessage.Define<T1, T2, T3>(
                    LogLevel.Warning,
                    eventId: 0,
                    formatString: messageTemplate
                )
            );

            //exception ??= new Exception(messageTemplate);
            log(logger, param1, param2, param3, exception);
        }
    }
}
