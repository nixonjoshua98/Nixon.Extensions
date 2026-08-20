# Nixon.Extensions

A collection of reusable .NET libraries for common application concerns, including configuration helpers, Entity Framework Core extensions, background jobs, OpenIddict integrations, and Serilog hosting support.

## Packages

| Package | Description |
| --- | --- |
| [Nixon.Extensions.Configuration](https://www.nuget.org/packages/Nixon.Extensions.Configuration) | Helper methods for strongly typed configuration access and required values. |
| [Nixon.Extensions.EntityFrameworkCore](https://www.nuget.org/packages/Nixon.Extensions.EntityFrameworkCore) | Extensions for model configuration and EF Core conventions. |
| [Nixon.Extensions.Hosting.Jobs](https://www.nuget.org/packages/Nixon.Extensions.Hosting.Jobs) | Cron-based background job registration for ASP.NET Core apps. |
| [Nixon.Extensions.OpenIddict](https://www.nuget.org/packages/Nixon.Extensions.OpenIddict) | Helpers and builders for OpenIddict server/client configuration. |
| [Nixon.Extensions.OpenIddict.EntityFrameworkCore](https://www.nuget.org/packages/Nixon.Extensions.OpenIddict.EntityFrameworkCore) | EF Core support for OpenIddict persistence and model setup. |
| [Nixon.Extensions.Serilog.AspNetCore](https://www.nuget.org/packages/Nixon.Extensions.Serilog.AspNetCore) | Logging setup helpers for ASP.NET Core applications using Serilog. |

## Features

- Strongly typed configuration access
- EF Core model builder extensions
- Scheduled jobs with cron expressions
- OpenIddict setup helpers for identity and OAuth flows
- Serilog integration for ASP.NET Core hosting
- Sample applications to help you get started quickly

## Installation

Install the package(s) you need with NuGet:

```bash
dotnet add package Nixon.Extensions.Configuration
dotnet add package Nixon.Extensions.Hosting.Jobs
```

## Samples

Included sample projects are located in the `samples` directory:

- `samples/Nixon.Extensions.Samples.Hosting.Jobs.Alpha`
- `samples/Nixon.Extensions.Samples.OpenIddict.Alpha`

## License

This project is licensed under the [MIT License](LICENSE.txt).
