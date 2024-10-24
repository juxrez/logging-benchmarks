using BenchmarkDotNet.Running;
using LoggingBenchmark;
using Microsoft.Extensions.Logging;

var summary = BenchmarkRunner.Run<LoggerBenchmark>();

