using Cronos;
using Microsoft.Extensions.DependencyInjection;

namespace Nixon.Extensions.Hosting.Jobs;

internal sealed class ScheduledCronJob<TJob>(string jobId, string expression) : IConfiguredJob 
    where TJob : class, IScheduledJob
{
    public string JobId { get; } = jobId;
    
    private readonly CronExpression _cronExpression = CronExpression.Parse(expression, CronFormat.IncludeSeconds);

    public async ValueTask WaitUntilNextExecutionAsync(TimeProvider timeProvider, CancellationToken cancellationToken)
    {
        var utcNow = timeProvider.GetUtcNow();
        
        var nextOccurrence = 
            _cronExpression.GetNextOccurrence(utcNow, TimeZoneInfo.Utc) 
            ?? throw new Exception($"No next occurrence found for cron expression '{_cronExpression}' after {utcNow}.");

        var delay = nextOccurrence - utcNow;

        if (delay <= TimeSpan.Zero)
        {
            return;
        }

        await Task.Delay(delay, timeProvider, cancellationToken);
    }

    public ValueTask<IScheduledJob> CreateJobInstanceAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var job = serviceProvider.GetRequiredService<TJob>();

        return ValueTask.FromResult<IScheduledJob>(job);
    }
}