using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace Nixon.Extensions.Serilog.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static T UseAdditionalSerilogRequestLogging<T>(
        this T app, 
        Action<AdditionalSerilogRequestLoggingOptions>? configure = null) 
        where T : IApplicationBuilder
    {
        app.UseSerilogRequestLogging(opts =>
        {
            var options = new AdditionalSerilogRequestLoggingOptions(opts);
            
            configure?.Invoke(options);
            
            var helper = new SerilogRequestLoggerHelper(options);
            
            opts.GetLevel = helper.GetLogEventLevel;
        });

        return app;
    }
}

file sealed class SerilogRequestLoggerHelper(AdditionalSerilogRequestLoggingOptions options)
{
    private readonly LogEventLevel _defaultLogLevel = options.DefaultLogLevel;
    private readonly LogEventLevel _exceptionLogLevel = options.ExceptionLogLevel;
    private readonly LogEventLevel _healthCheckLogLevel = options.HealthCheckLogLevel;
    private readonly LogEventLevel _successfulResponseLogLevel = options.SuccessfulResponseLogLevel;

    private static bool IsHealthCheck(HttpContext ctx)
    {
        var endpoint = ctx.GetEndpoint();

        if (endpoint is null || string.IsNullOrEmpty(endpoint.DisplayName)) return true;

        return endpoint.DisplayName.Contains("health check", StringComparison.OrdinalIgnoreCase);
    }

    public LogEventLevel GetLogEventLevel(HttpContext ctx, double _, Exception? ex)
    {
        if (ex is not null)
        {
            return _exceptionLogLevel;
        }

        if (ctx.Response.StatusCode is >= 200 and < 300)
        {
            return _successfulResponseLogLevel;
        }
        
        if (IsHealthCheck(ctx))
        {
            return _healthCheckLogLevel;
        }

        return _defaultLogLevel;
    }
}