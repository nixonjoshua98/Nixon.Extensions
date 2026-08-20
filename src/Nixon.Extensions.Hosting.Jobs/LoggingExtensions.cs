using Microsoft.Extensions.Logging;

namespace Nixon.Extensions.Hosting.Jobs;

internal static partial class LoggingExtensions
{
    [LoggerMessage(LogLevel.Information, "Started processing {jobCount} jobs.")]
    public static partial void LogStartedProcessingJobs(this ILogger<ExecutionBackgroundService> logger, int jobCount);

    [LoggerMessage(LogLevel.Information, "Started executing job {jobId}.")]
    public static partial void LogStartedExecutingJob(this ILogger<ExecutionBackgroundService> logger, string jobId);

    [LoggerMessage(LogLevel.Information, "Finished executing job {jobId}.")]
    public static partial void LogFinishedExecutingJob(this ILogger<ExecutionBackgroundService> logger, string jobId);

    [LoggerMessage(LogLevel.Error, "Error executing job {jobId}.")]
    public static partial void LogErrorExecutingJob(this ILogger<ExecutionBackgroundService> logger, Exception exception, string jobId);
}