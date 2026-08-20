
using Nixon.Extensions.Hosting.Jobs;
using Nixon.Extensions.Samples.Hosting.Jobs.Alpha;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddCronJob<PingJob>("*/5 * * * * *");

var app = builder.Build();

await app.RunAsync();