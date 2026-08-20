using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Nixon.Extensions.OpenIddict.Configuration;
using OpenIddict.Server;

namespace Nixon.Extensions.OpenIddict.Builders;

public sealed class OpenIddictOpinionatedServerBuilder(IHostEnvironment environment)
{
    private bool _hasSigningMethod;
    private bool _hasEncryptionMethod;
    
    private Action<OpenIddictServerBuilder>? _configureAction;
    
    public OpenIddictOpinionatedServerBuilder SetIssuer(string issuer) =>
        AppendConfiguration(x => x.SetIssuer(issuer));
    
    public OpenIddictOpinionatedServerBuilder AddApplication(Action<OpenIddictApplicationRegistration> configure) =>
        AppendConfiguration(x => x.Services.AddOpenIddictApplication(configure));
    
    public OpenIddictOpinionatedServerBuilder AddApplication(OpenIddictApplicationRegistration registration) =>
        AppendConfiguration(x => x.Services.AddOpenIddictApplication(registration));
    
    public OpenIddictOpinionatedServerBuilder AddApplications<TSource>(
        IEnumerable<TSource> source,
        Action<TSource, OpenIddictApplicationRegistration> configure) =>
        AppendConfiguration(x => x.Services.AddOpenIddictApplications(source, configure));
    
    public OpenIddictOpinionatedServerBuilder AddScopedTokenRequestHandler<THandler>()
        where THandler : class, IOpenIddictServerHandler<OpenIddictServerEvents.HandleTokenRequestContext> =>
        AppendConfiguration(x => x.AddScopedTokenRequestHandler<THandler>());
    
    public OpenIddictOpinionatedServerBuilder AddSigningKey(SecurityKey key)
    {
        _hasSigningMethod = true;
        
        return AppendConfiguration(x => x.AddSigningKey(key));
    }
    
    public OpenIddictOpinionatedServerBuilder AddEncryptionKey(SecurityKey key)
    {
        _hasEncryptionMethod = true;
        
        return AppendConfiguration(x => x.AddEncryptionKey(key));
    }
    
    public OpenIddictOpinionatedServerBuilder AllowCustomFlows(IEnumerable<string> grantTypes) =>
        AppendConfiguration(x => x.AllowCustomFlows(grantTypes));
    
    public OpenIddictOpinionatedServerBuilder AllowCustomFlow(string grantType) =>
        AppendConfiguration(x => x.AllowCustomFlow(grantType));
    
    public OpenIddictOpinionatedServerBuilder AllowClientCredentialsFlow() =>
        AppendConfiguration(x => x.AllowClientCredentialsFlow());
    
    public OpenIddictOpinionatedServerBuilder AddClientCredentialsFlowHandler() =>
        AppendConfiguration(x => x.AddClientCredentialsFlowHandler());

    private OpenIddictOpinionatedServerBuilder AppendConfiguration(Action<OpenIddictServerBuilder> configure)
    {
        _configureAction += configure;

        return this;
    }
    
    public void Configure(OpenIddictServerBuilder builder)
    {
        if (!_hasSigningMethod)
        {
            builder.AddDevelopmentSigningCertificate();
        }

        if (!_hasEncryptionMethod)
        {
            builder.AddDevelopmentEncryptionCertificate();
        }
        
        builder.UseDataProtection();
        builder.UseReferenceAccessTokens();
        
        builder.SetAccessTokenLifetime(TimeSpan.FromHours(1));

        builder.AllowTokenExchangeFlow();
        builder.AllowAuthorizationCodeFlow();
        
        builder.AllowRefreshTokenFlow(TimeSpan.FromDays(14));

        builder.SetTokenEndpointUris("connect/token");
        builder.SetUserInfoEndpointUris("connect/userinfo");
        builder.SetAuthorizationEndpointUris("connect/authorize");

        builder.UseAspNetCore(asp => asp
            .EnableUserInfoEndpointPassthrough()
            .EnableAuthorizationEndpointPassthrough()
            .DisableDevelopmentTransportSecurityRequirement(environment)
        );

        _configureAction?.Invoke(builder);
    }
}