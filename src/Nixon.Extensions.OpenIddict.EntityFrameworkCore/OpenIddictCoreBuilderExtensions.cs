using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Nixon.Extensions.OpenIddict.EntityFrameworkCore;

public static class OpenIddictCoreBuilderExtensions
{
    public static OpenIddictCoreBuilder UseEntityFrameworkCore<TContext>(
        this OpenIddictCoreBuilder builder,
        Action<OpenIddictEntityFrameworkCoreBuilder>? configure = null)
        where TContext : DbContext
    {
        return builder.UseEntityFrameworkCore(options =>
        {
            options.UseDbContext<TContext>();
            
            configure?.Invoke(options);
        });
    }
}