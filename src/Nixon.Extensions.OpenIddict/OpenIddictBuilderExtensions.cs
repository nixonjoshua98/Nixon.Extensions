using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nixon.Extensions.OpenIddict.Builders;

namespace Nixon.Extensions.OpenIddict;

public static class OpenIddictBuilderExtensions
{
    public static OpenIddictBuilder AddOpinionatedServer(
        this OpenIddictBuilder builder,
        IHostEnvironment environment,
        Action<OpenIddictOpinionatedServerBuilder>? configure = null)
    {
        builder.AddServer(server =>
        {
            var opinionatedBuilder = new OpenIddictOpinionatedServerBuilder(
                environment
            );

            configure?.Invoke(opinionatedBuilder);
            
            opinionatedBuilder.Configure(server);
        });
        
        return builder;
    }
    
    public static OpenIddictBuilder AddOpinionatedClient(
        this OpenIddictBuilder builder,
        IHostEnvironment environment,
        Action<OpenIddictOpinionatedClientBuilder>? configure = null)
    {
        var opinionatedBuilder = new OpenIddictOpinionatedClientBuilder(environment);

        configure?.Invoke(opinionatedBuilder);

        builder.AddClient(opinionatedBuilder.Configure);
        
        return builder;
    }
}