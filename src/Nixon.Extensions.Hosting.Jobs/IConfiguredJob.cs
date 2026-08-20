namespace Nixon.Extensions.Hosting.Jobs;

internal interface IConfiguredJob
{
    string JobId { get; }
    
    ValueTask WaitUntilNextExecutionAsync(TimeProvider timeProvider, CancellationToken cancellationToken);
    
    ValueTask<IScheduledJob> CreateJobInstanceAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
}