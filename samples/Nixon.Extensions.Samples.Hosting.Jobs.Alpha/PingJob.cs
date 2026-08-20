using Microsoft.Extensions.Logging;
using Nixon.Extensions.Hosting.Jobs;

namespace Nixon.Extensions.Samples.Hosting.Jobs.Alpha;

public sealed class PingJob(ILogger<PingJob> logger) : IScheduledJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Ping");
        
        return Task.CompletedTask;
    }
}