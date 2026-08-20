using System.Security.Claims;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;

namespace Nixon.Extensions.OpenIddict.Handlers;

internal sealed class ClientCredentialsFlowHandler() :  
    IOpenIddictServerHandler<OpenIddictServerEvents.HandleTokenRequestContext>
{
    public ValueTask HandleAsync(OpenIddictServerEvents.HandleTokenRequestContext context)
    {
        if (!context.Request.IsClientCredentialsGrantType())
        {
            return ValueTask.CompletedTask;
        }

        var clientId = context.Request.ClientId!;
        
        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        
        identity.AddClaim(OpenIddictConstants.Claims.Subject, clientId);
        
        identity.SetDestinations(_ => [OpenIddictConstants.Destinations.AccessToken]);
        
        var principal = new ClaimsPrincipal(identity);
        
        principal.SetScopes(context.Request.GetScopes());
        
        context.Principal = principal;

        return ValueTask.CompletedTask;
    }
}