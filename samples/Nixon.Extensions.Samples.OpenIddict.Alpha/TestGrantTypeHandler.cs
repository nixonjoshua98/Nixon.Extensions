using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace Nixon.Extensions.Samples.OpenIddict.Alpha;

internal sealed class TestGrantTypeHandler : IOpenIddictServerHandler<HandleTokenRequestContext>
{
    public ValueTask HandleAsync(HandleTokenRequestContext context)
    {
        if (context.Request.GrantType != "test-grant-type")
        {
            return ValueTask.CompletedTask;
        }

        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType);
        
        identity.AddClaim(OpenIddictConstants.Claims.Subject, "test-user-id");

        context.Principal = new ClaimsPrincipal(identity);

        return ValueTask.CompletedTask;
    }
}