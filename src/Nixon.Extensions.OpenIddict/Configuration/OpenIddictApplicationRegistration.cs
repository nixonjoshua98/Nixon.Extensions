using OpenIddict.Abstractions;

namespace Nixon.Extensions.OpenIddict.Configuration;

internal sealed class OpenIddictOptions
{
    public readonly List<OpenIddictApplicationRegistration> ApplicationRegistrations = [];
}

public sealed class OpenIddictApplicationRegistration
{
    public string ClientId { get; set; } = string.Empty;
    
    public string? ClientSecret { get; set; } = null;
    
    public string ClientType { get; set; } = string.Empty;

    public List<string> AllowedGrantTypes { get; private set; } = [];
    
    public List<string> SupportedScopes { get; private set; } = [];
    
    public List<string> RedirectUris { get; private set; } = [];
    
    public List<string> AllowedEndpoints { get; private set; } = [];
    
    public bool UpdateIfExists { get; set; } = true;
    
    public OpenIddictApplicationRegistration WithSupportedScopes(IEnumerable<string> scopes)
    {
        SupportedScopes.AddRange(scopes);
        return this;
    }

    public OpenIddictApplicationRegistration WithEndpoints(IEnumerable<string> endpoints)
    {
        AllowedEndpoints.AddRange(endpoints);
        return this;
    }
    
    public OpenIddictApplicationRegistration WithRedirectUris(IEnumerable<string> redirectUris)
    {
        RedirectUris.AddRange(redirectUris);
        return this;
    }

    public OpenIddictApplicationRegistration WithClientId(string clientId)
    {
        ClientId = clientId;
        return this;
    }
    
    public OpenIddictApplicationRegistration WithClientSecret(string clientSecret)
    {
        ClientSecret = clientSecret;
        return this;
    }
    
    public OpenIddictApplicationRegistration WithClientType(string clientType)
    {
        ClientType = clientType;
        return this;
    }
    
    public OpenIddictApplicationRegistration WithAllowedGrantTypes(IEnumerable<string> grantTypes)
    {
        AllowedGrantTypes.AddRange(grantTypes);
        return this;
    }
    
    public OpenIddictApplicationDescriptor CreateDescriptor()
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            ClientType = ClientType,
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.Endpoints.Authorization,

                OpenIddictConstants.Permissions.ResponseTypes.Code
            }
        };

        descriptor.AddEndpointPermissions(AllowedEndpoints);

        descriptor.AddScopePermissions(SupportedScopes);
        
        descriptor.AddGrantTypePermissions(AllowedGrantTypes);
        
        descriptor.AddRedirectUris(RedirectUris);
        
        return descriptor;
    }
}