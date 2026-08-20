using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nixon.Extensions.OpenIddict.Configuration;
using Nixon.Extensions.OpenIddict.Handlers;
using OpenIddict.Server;

namespace Nixon.Extensions.OpenIddict;

public static class OpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AllowRefreshTokenFlow(
        this OpenIddictServerBuilder builder, 
        TimeSpan refreshTokenLifetime)
    {
        return builder
            .AllowRefreshTokenFlow()
            .SetRefreshTokenLifetime(refreshTokenLifetime);
    }
    
    public static OpenIddictServerBuilder AddApplications<TSource>(
        this OpenIddictServerBuilder builder,
        IEnumerable<TSource> source,
        Action<TSource, OpenIddictApplicationRegistration> configure)
    {
        builder.Services.AddOpenIddictApplications(source, configure);
        
        return builder;
    }
    
    public static OpenIddictServerAspNetCoreBuilder DisableDevelopmentTransportSecurityRequirement(
        this OpenIddictServerAspNetCoreBuilder builder,
        IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            builder.DisableTransportSecurityRequirement();
        }

        return builder;
    }

    public static OpenIddictServerBuilder AllowCustomFlows(
        this OpenIddictServerBuilder builder, 
        IEnumerable<string> customFlows)
    {
        foreach (var customFlow in customFlows)
        {
            builder.AllowCustomFlow(customFlow);
        }
            
        return builder;
    }

    public static OpenIddictServerBuilder AddClientCredentialsFlowHandler(
        this OpenIddictServerBuilder builder)
    {
        return builder.AddScopedTokenRequestHandler<ClientCredentialsFlowHandler>();
    }
    
    public static OpenIddictServerBuilder AddScopedProcessErrorHandler<THandler>(this OpenIddictServerBuilder builder)
        where THandler : class, IOpenIddictServerHandler<OpenIddictServerEvents.ProcessErrorContext>
    {
        return builder
            .AddEventHandler<OpenIddictServerEvents.ProcessErrorContext>(x => x.UseScopedHandler<THandler>());
    }

    public static OpenIddictServerBuilder AddScopedTokenRequestHandler<THandler>(this OpenIddictServerBuilder builder)
        where THandler : class, IOpenIddictServerHandler<OpenIddictServerEvents.HandleTokenRequestContext>
    {
        return builder
            .AddEventHandler<OpenIddictServerEvents.HandleTokenRequestContext>(x => x.UseScopedHandler<THandler>());
    }
}