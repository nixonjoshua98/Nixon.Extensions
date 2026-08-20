using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nixon.Extensions.OpenIddict.Configuration;
using OpenIddict.Abstractions;

namespace Nixon.Extensions.OpenIddict.BackgroundServices;

internal sealed class ApplicationRegistrationBackgroundService(
    IOptions<OpenIddictOptions> options,
    IServiceProvider serviceProvider,
    ILogger<ApplicationRegistrationBackgroundService> logger
) : IHostedService
{
    private readonly OpenIddictOptions _options = options.Value;
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        foreach (var application in _options.ApplicationRegistrations)
        {
            var descriptor = application.CreateDescriptor();
            
            var existing = await manager.FindByClientIdAsync(application.ClientId, cancellationToken);

            if (existing is null)
            {
                await manager.CreateAsync(descriptor, cancellationToken);
                
                logger.LogInformation("Created application with client id '{ClientId}'", application.ClientId);
            }
            else if (application.UpdateIfExists)
            {
                await manager.UpdateAsync(existing, descriptor, cancellationToken);
                
                logger.LogInformation("Updated application with client id '{ClientId}'", application.ClientId);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}