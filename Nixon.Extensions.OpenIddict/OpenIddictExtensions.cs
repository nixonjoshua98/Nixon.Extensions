using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

namespace Nixon.Extensions.OpenIddict;

public static class OpenIddictExtensions
{
    public static OpenIddictServerBuilder AllowRefreshTokenFlow(this OpenIddictServerBuilder builder, TimeSpan refreshTokenLifetime)
    {
        return builder
            .AllowRefreshTokenFlow()
            .SetRefreshTokenLifetime(refreshTokenLifetime);
    }

    public static OpenIddictServerBuilder AllowCustomFlows(this OpenIddictServerBuilder builder, IEnumerable<string> customFlows)
    {
        foreach (var customFlow in customFlows)
        {
            builder.AllowCustomFlow(customFlow);
        }
            
        return builder;
    }

    public static bool TryGetParameter<T>(this OpenIddictRequest request, string name, [NotNullWhen(true)] out T? value) 
        where T : IParsable<T>
    {
        return TryGetParameter(request, name, CultureInfo.InvariantCulture, out value);
    }

    public static bool TryGetParameter<T>(this OpenIddictRequest request, string name, IFormatProvider provider, [NotNullWhen(true)] out T? value) 
        where T : IParsable<T>
    {
        ArgumentNullException.ThrowIfNull(request);
            
        value = default;

        if (!request.TryGetParameter(name, out var param))
        {
            return false;
        }
            
        var strParam = param.ToString();

        return T.TryParse(strParam, provider, out value);
    }

    public static async Task<IOpenIddictApplicationManager> CreateOrUpdateAsync(this IOpenIddictApplicationManager manager, OpenIddictApplicationDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(descriptor.ClientId, "ClientId");

        var byClientIdAsync = await manager.FindByClientIdAsync(descriptor.ClientId, cancellationToken);

        if (byClientIdAsync != null)
        {
            await manager.UpdateAsync(byClientIdAsync, descriptor, cancellationToken);
        }
        else
        {
            await manager.CreateAsync(descriptor, cancellationToken);
        }

        return manager;
    }

    public static OpenIddictApplicationDescriptor AddRedirectUris(this OpenIddictApplicationDescriptor descriptor, IEnumerable<Uri> redirectUris)
    {
        foreach (var redirectUri in redirectUris)
        {
            descriptor.RedirectUris.Add(redirectUri);
        }
            
        return descriptor;
    }

    public static OpenIddictApplicationDescriptor AddRedirectUris(this OpenIddictApplicationDescriptor descriptor, IEnumerable<string> redirectUris)
    {
        foreach (var redirectUri in redirectUris)
        {
            descriptor.RedirectUris.Add(new Uri(redirectUri));
        }
            
        return descriptor;
    }
}