using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Nixon.Extensions.Hosting;

public static class HostBuilderExtensions
{
    public static T AddSerilogConfiguration<T>(this T builder)
        where T : IHostApplicationBuilder
    {
        builder.Services
            .AddSerilog((_, config) =>
                config
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
            );

        return builder;
    }
    
    public static T UseConfiguredRequestLogging<T>(this T app)
        where T : IApplicationBuilder
    {
        app.UseSerilogRequestLogging(opts => opts.GetLevel = RequestLoggingHelper.GetLogEventLevel);

        return app;
    }
    
    public static T UseNonRestriveCors<T>(this T app) where T : IApplicationBuilder
    {
        app.UseCors(builder =>
        {
            builder
                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                .AllowAnyHeader()
                .AllowAnyOrigin();
        });

        return app;
    }
}

file static class RequestLoggingHelper
{
    private static bool IsInfraEndpoint(HttpContext ctx)
    {
        var endpoint = ctx.GetEndpoint();

        if (endpoint is null || string.IsNullOrEmpty(endpoint.DisplayName)) return true;

        return endpoint.DisplayName.Contains("health check", StringComparison.OrdinalIgnoreCase);
    }

    public static LogEventLevel GetLogEventLevel(HttpContext ctx, double _, Exception? ex)
    {
        if (ex is not null)
        {
            return LogEventLevel.Error;
        }

        else if (ctx.Response.StatusCode is < 200 or >= 300)
        {
            return LogEventLevel.Warning;
        }

        else if (IsInfraEndpoint(ctx)) return LogEventLevel.Debug;

        return LogEventLevel.Information;
    }
}