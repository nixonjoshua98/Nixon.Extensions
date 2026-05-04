using Serilog.AspNetCore;
using Serilog.Events;

namespace Nixon.Serilog.AspNetCore.Extensions;

public sealed class AdditionalSerilogRequestLoggingOptions(RequestLoggingOptions options)
{
    public RequestLoggingOptions Options { get; } = options;
    
    public LogEventLevel ExceptionLogLevel { get; set; } = LogEventLevel.Error;
    
    public LogEventLevel SuccessfulResponseLogLevel { get; set; } = LogEventLevel.Information;
    
    public LogEventLevel HealthCheckLogLevel { get; set; } = LogEventLevel.Information;
    
    public LogEventLevel DefaultLogLevel { get; set; } = LogEventLevel.Information;
}