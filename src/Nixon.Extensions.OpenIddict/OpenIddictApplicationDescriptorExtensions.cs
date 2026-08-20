using OpenIddict.Abstractions;

namespace Nixon.Extensions.OpenIddict;

public static class OpenIddictApplicationDescriptorExtensions
{
    public static OpenIddictApplicationDescriptor AddGrantTypePermissions(
        this OpenIddictApplicationDescriptor descriptor, 
        IEnumerable<string> grantTypes)
    {
        foreach (var grantType in grantTypes)
        {
            descriptor.AddGrantTypePermissions(grantType);
        }
            
        return descriptor;
    }
    
    public static OpenIddictApplicationDescriptor AddRedirectUris(
        this OpenIddictApplicationDescriptor descriptor, 
        IEnumerable<Uri> redirectUris)
    {
        foreach (var redirectUri in redirectUris)
        {
            descriptor.RedirectUris.Add(redirectUri);
        }
            
        return descriptor;
    }
    
    public static OpenIddictApplicationDescriptor AddScopePermissions(
        this OpenIddictApplicationDescriptor descriptor, 
        IEnumerable<string> scopes)
    {
        foreach (var scope in scopes)
        {
            descriptor.AddScopePermissions(scope);
        }
            
        return descriptor;
    }
    
    public static OpenIddictApplicationDescriptor AddEndpointPermissions(
        this OpenIddictApplicationDescriptor descriptor, 
        IEnumerable<string> endpoints)
    {
        foreach (var endpoint in endpoints)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Endpoint + endpoint);
        }
            
        return descriptor;
    }

    public static OpenIddictApplicationDescriptor AddRedirectUris(
        this OpenIddictApplicationDescriptor descriptor, 
        IEnumerable<string> redirectUris)
    {
        foreach (var redirectUri in redirectUris)
        {
            descriptor.RedirectUris.Add(new Uri(redirectUri));
        }
            
        return descriptor;
    }
}