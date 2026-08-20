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
    private readonly LogEventLevel _errorLogLevel = options.ErrorLogLevel;
    private readonly LogEventLevel _defaultLogLevel = options.DefaultLogLevel;
    private readonly LogEventLevel _metricsLogLevel = options.MetricsLogLevel;
    private readonly LogEventLevel _healthCheckLogLevel = options.HealthCheckLogLevel;
    private readonly LogEventLevel _successfulResponseLogLevel = options.SuccessfulResponseLogLevel;
    
    private readonly Func<HttpRequest, bool> _metricsPredicate = options.MetricsPredicate;

    private static bool IsHealthCheck(HttpContext ctx)
    {
        var endpoint = ctx.GetEndpoint();

        if (endpoint is null || string.IsNullOrEmpty(endpoint.DisplayName)) return false;
        
        return endpoint.DisplayName.Contains("health check", StringComparison.OrdinalIgnoreCase);
    }

    public LogEventLevel GetLogEventLevel(HttpContext ctx, double _, Exception? ex)
    {
        if (ex is not null || ctx.Response.StatusCode >= 400)
        {
            return _errorLogLevel;
        }
        
        if (IsHealthCheck(ctx))
        {
            return _healthCheckLogLevel;
        }

        if (_metricsPredicate(ctx.Request))
        {
            return _metricsLogLevel;
        }

        if (ctx.Response.StatusCode is >= 200 and < 300)
        {
            return _successfulResponseLogLevel;
        }

        return _defaultLogLevel;
    }
}