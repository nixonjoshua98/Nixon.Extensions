using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nixon.Extensions.Hosting.Jobs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCronJob<TJob>(
        this IServiceCollection services,
        string expression)
        where TJob : class, IScheduledJob
    {
        return services.AddCronJob<TJob>(typeof(TJob).FullName!, expression);
    }
    
    public static IServiceCollection AddCronJob<TJob>(
        this IServiceCollection services,
        string jobId,
        string expression)
        where TJob : class, IScheduledJob
    {
        var cronJob = new ScheduledCronJob<TJob>(jobId, expression);
        
        services.AddHostedService<ExecutionBackgroundService>();

        services.Configure<JobStateOptions>(options =>
        {
            options.Jobs.Add(jobId, cronJob);
        });
        
        services.TryAddTransient<TJob>();

        return services;
    }
}