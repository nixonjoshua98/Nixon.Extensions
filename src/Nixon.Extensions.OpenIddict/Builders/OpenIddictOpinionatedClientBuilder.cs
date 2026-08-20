using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Nixon.Extensions.OpenIddict.Builders;

public sealed class OpenIddictOpinionatedClientBuilder(IHostEnvironment environment)
{
    private Action<OpenIddictClientBuilder>? _configureAction;

    private bool _hasSigningMethod;
    private bool _hasEncryptionMethod;
    
    public OpenIddictOpinionatedClientBuilder UseWebProviders(Action<OpenIddictClientWebIntegrationBuilder> action)
    {
        return AppendConfiguration(x => x.UseWebProviders(action));
    }

    public OpenIddictOpinionatedClientBuilder AddSigningKey(SecurityKey key)
    {
        _hasSigningMethod = true;
        
        return AppendConfiguration(x => x.AddSigningKey(key));
    }
    
    public OpenIddictOpinionatedClientBuilder AddEncryptionKey(SecurityKey key)
    {
        _hasEncryptionMethod = true;
        
        return AppendConfiguration(x => x.AddEncryptionKey(key));
    }

    internal void Configure(OpenIddictClientBuilder builder)
    {
        if (!_hasSigningMethod)
        {
            builder.AddDevelopmentSigningCertificate();
        }
        
        if (!_hasEncryptionMethod)
        {
            builder.AddDevelopmentEncryptionCertificate();
        }

        builder.SetRedirectionEndpointUris("connect/redirect");

        builder.AllowAuthorizationCodeFlow();

        builder.UseDataProtection();

        builder.UseSystemNetHttp();
        
        builder.UseAspNetCore(asp => asp
            .EnableRedirectionEndpointPassthrough()
            .DisableDevelopmentTransportSecurityRequirement(environment)
        );

        _configureAction?.Invoke(builder);
    }
    
    private OpenIddictOpinionatedClientBuilder AppendConfiguration(Action<OpenIddictClientBuilder> configure)
    {
        _configureAction += configure;

        return this;
    }
}