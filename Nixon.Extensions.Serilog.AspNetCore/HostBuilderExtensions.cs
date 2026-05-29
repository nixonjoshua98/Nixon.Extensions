using Microsoft.Extensions.Hosting;
using Serilog;

namespace Nixon.Extensions.Serilog.AspNetCore;

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
}