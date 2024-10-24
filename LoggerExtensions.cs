using Microsoft.Extensions.Logging;

namespace LoggingBenchmark
{
    public static class LoggerExtensions
    {
        
        public static void Log_Information<T0, T1, T2>(this ILogger logger, string message, T0 arg0, T1 arg1, T2 arg2)
        {
            Action<ILogger, T0, T1, T2, Exception?> logInformation =
                LoggerMessage.Define<T0, T1, T2>(LogLevel.Information, eventId: 0, formatString: message);

            logInformation(logger, arg0, arg1, arg2, null);
        }

        public static void Log_Warning<T0, T1, T2>(this ILogger logger, string message, T0 arg0, T1 arg1, T2 arg2)
        {
            Action<ILogger, T0, T1, T2, Exception?> logInformation =
                LoggerMessage.Define<T0, T1, T2>(LogLevel.Warning, eventId: 0, formatString: message);

            logInformation(logger, arg0, arg1, arg2, null);
        }
    }
}
