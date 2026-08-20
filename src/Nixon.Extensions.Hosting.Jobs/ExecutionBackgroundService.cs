using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Nixon.Extensions.Hosting.Jobs;

internal sealed class ExecutionBackgroundService(
    IOptions<JobStateOptions> options,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ExecutionBackgroundService> logger
) : BackgroundService
{
    private readonly JobStateOptions _stateOptions = options.Value;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tasks = _stateOptions.Jobs.Values
            .Select(job => ProcessJobAsync(job, stoppingToken))
            .ToArray();
        
        logger.LogStartedProcessingJobs(tasks.Length);

        await Task.WhenAll(tasks);
    }

    private async Task ProcessJobAsync(IConfiguredJob configuredJob, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await configuredJob.WaitUntilNextExecutionAsync(TimeProvider.System, cancellationToken);
            
            cancellationToken.ThrowIfCancellationRequested();
            
            await ExecuteJobAsync(configuredJob, cancellationToken);
        }
    }

    private async Task ExecuteJobAsync(IConfiguredJob configuredJob, CancellationToken cancellationToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        
        logger.LogStartedExecutingJob(configuredJob.JobId);
        
        try
        {
            var scheduledJob = await configuredJob.CreateJobInstanceAsync(
                scope.ServiceProvider, 
                cancellationToken
            );

            await scheduledJob.ExecuteAsync(cancellationToken);
            
            logger.LogFinishedExecutingJob(configuredJob.JobId);
        }
        catch (Exception exception)
        {
            logger.LogErrorExecutingJob(exception, configuredJob.JobId);
        }
    }
}