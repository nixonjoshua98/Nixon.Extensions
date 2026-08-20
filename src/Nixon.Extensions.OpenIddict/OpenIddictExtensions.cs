using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

namespace Nixon.Extensions.OpenIddict;

public static class OpenIddictExtensions
{
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
}