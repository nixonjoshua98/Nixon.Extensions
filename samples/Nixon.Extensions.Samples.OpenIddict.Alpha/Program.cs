using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Nixon.Extensions.OpenIddict;
using Nixon.Extensions.OpenIddict.Configuration;
using Nixon.Extensions.Samples.OpenIddict.Alpha;
using Nixon.Extensions.OpenIddict.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("Samples.Alpha");
});

builder.Services.AddOpenIddict()
    .AddCore(core =>
    {
        core.UseEntityFrameworkCore<AppDbContext>();
    })
    .AddOpinionatedServer(builder.Environment, server =>
    {
        server.SetIssuer("http://localhost:5000");

        server.AllowClientCredentialsFlow();

        server.AddClientCredentialsFlowHandler();

        server.AddScopedTokenRequestHandler<TestGrantTypeHandler>();

        server.AllowCustomFlow("test-grant-type");
        
        server.AddApplication(new OpenIddictApplicationRegistration()
        {
            ClientId = "client-id",
            AllowedGrantTypes = { "test-grant-type" },
        });
        
        server.AddApplication(new OpenIddictApplicationRegistration()
        {
            ClientId = "client-flow-client",
            ClientSecret = "client-flow-secret",
            AllowedGrantTypes = { "client_credentials", "refresh_token" },
        });
    })
    .AddOpinionatedClient(builder.Environment)
    .AddValidation(validation =>
    {
        validation.SetIssuer("http://localhost:5000");

        validation.UseAspNetCore();
        validation.UseLocalServer();
        validation.UseSystemNetHttp();
        validation.UseDataProtection();
    });

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

var app = builder.Build();

await app.RunAsync();