using Microsoft.AspNetCore.Http;
using Serilog.AspNetCore;
using Serilog.Events;

namespace Nixon.Extensions.Serilog.AspNetCore;

public sealed class AdditionalSerilogRequestLoggingOptions(RequestLoggingOptions options)
{
    public RequestLoggingOptions Options { get; } = options;
    
    public LogEventLevel ErrorLogLevel { get; set; } = LogEventLevel.Error;
    
    public LogEventLevel SuccessfulResponseLogLevel { get; set; } = LogEventLevel.Information;
    
    public LogEventLevel HealthCheckLogLevel { get; set; } = LogEventLevel.Debug;
    
    public LogEventLevel DefaultLogLevel { get; set; } = LogEventLevel.Information;
    
    public LogEventLevel MetricsLogLevel { get; set; } = LogEventLevel.Debug;

    public Func<HttpRequest, bool> MetricsPredicate { get; set; } =
        static r => r.Path.StartsWithSegments("/metrics");
}