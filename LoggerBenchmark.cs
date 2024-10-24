using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;

namespace LoggingBenchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    public class LoggerBenchmark
    {
        private readonly ILogger<LoggerBenchmark> _logger;

        private static readonly Action<ILogger, int, int, int, Exception?> _logWarningWithParamsDelegate =
            LoggerMessage.Define<int, int, int>(
                LogLevel.Warning,
                eventId: 0, 
                "Logging a message with 3 parameters: {Number1},{Number2},{Number3}");
                private static readonly Action<ILogger, Exception?> _LoggerMessageNoParams =             LoggerMessage.Define(                LogLevel.Warning, 
                eventId: 0, 
                "This is a message with no params!");

        private readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Warning);
        });

        public LoggerBenchmark()
        {
            _logger = new Logger<LoggerBenchmark>(loggerFactory);
        }

        [Benchmark]
        public void UsingLogOriginal_WithParameters()
        {
            try
            {
                TestFunction();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Logging a message with 3 parameters: {Number1},{Number2},{Number3}", 1, 2, 3);
            }
        }

        [Benchmark]
        public void UsingLogDefine_LogWarning_WithoutParameters()
        {
            try
            {
                TestFunction();
            }
            catch (Exception ex)
            {
                _LoggerMessageNoParams(_logger, ex);
            }
        }

        [Benchmark]
        public void UsingLogDefine_LogWarning_WithParameters()
        {
            try
            {
                TestFunction();
            }
            catch (Exception ex)
            {
                _logWarningWithParamsDelegate(_logger, 1, 2, 3, ex);
            }
        }

        [Benchmark]
        public void UsingMemoized_LogWarning_WithParameters()
        {
            try
            {
                TestFunction();
            }
            catch (Exception ex)
            {
                _logger.MemoizeLogWarning("Logging a message with 3 parameters: {Number1},{Number2},{Number3}", 1, 2, 3, ex);
            }
        }

        private void TestFunction() => throw new ArgumentNullException(nameof(TestFunction));   
    }
}
