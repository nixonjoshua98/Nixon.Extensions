namespace Nixon.Extensions.Hosting.Jobs;

public interface IScheduledJob
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}