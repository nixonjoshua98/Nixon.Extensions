using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Nixon.Extensions.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredPostgresDbContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        Action<DbContextOptionsBuilder>? optionsFunc = null
    ) where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        AddConfiguredPostgresDbContext<TContext>(services, connectionString, environment, optionsFunc);

        return services;
    }

    private static IServiceCollection AddConfiguredPostgresDbContext<TContext>(
        IServiceCollection services,
        string? connectionString,
        IHostEnvironment environment,
        Action<DbContextOptionsBuilder>? optionsFunc = null
    ) where TContext : DbContext
    {
        services.AddDbContextFactory<TContext>((provider, options) =>
        {
            options.UseNpgsql(connectionString, opt =>
            {
                opt.MigrationsAssembly(typeof(TContext).Assembly.FullName);

                opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                opt.EnableRetryOnFailure(3, TimeSpan.FromMilliseconds(250), []);

                opt.CommandTimeout(10);
            });

            options.UseSnakeCaseNamingConvention();

            options.EnableSensitiveDataLogging(environment.IsDevelopment());

            optionsFunc?.Invoke(options);
        });
        
        return services;
    }
}