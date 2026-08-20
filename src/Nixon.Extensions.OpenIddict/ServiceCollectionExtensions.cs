using Microsoft.Extensions.DependencyInjection;
using Nixon.Extensions.OpenIddict.BackgroundServices;
using Nixon.Extensions.OpenIddict.Configuration;

namespace Nixon.Extensions.OpenIddict;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenIddictApplication(
        this IServiceCollection services,
        Action<OpenIddictApplicationRegistration> configure)
    {
        var registration = new OpenIddictApplicationRegistration();
            
        configure(registration);

        services.AddOpenIddictApplication(registration);
        
        return services;
    }
    
    public static IServiceCollection AddOpenIddictApplications<TSource>(
        this IServiceCollection services,
        IEnumerable<TSource> source,
        Action<TSource, OpenIddictApplicationRegistration> configure)
    {
        foreach (var item in source)
        {
            var registration = new OpenIddictApplicationRegistration();
            
            configure(item, registration);

            services.AddOpenIddictApplication(registration);
        }
        
        return services;
    }
    
    public static IServiceCollection AddOpenIddictApplication(
        this IServiceCollection services,
        OpenIddictApplicationRegistration registration)
    {
        services.AddHostedService<ApplicationRegistrationBackgroundService>();
        
        services.Configure<OpenIddictOptions>(options =>
        {
            options.ApplicationRegistrations.Add(registration);
        });
        
        return services;
    }
}