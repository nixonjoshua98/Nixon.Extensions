using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenIddict.EntityFrameworkCore.Models;

// ReSharper disable InvertIf

namespace Nixon.Extensions.OpenIddict.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void ConfigureOpenIddict(
        this ModelBuilder modelBuilder,
        string schemaName,
        string? applicationTableName = null,
        string? authorizationTableName = null,
        string? scopeTableName = null,
        string? tokenTableName = null)
    {
        modelBuilder.ConfigureOpenIddict(
            application =>
            {
                application.SetSchema(schemaName);
                
                if (!string.IsNullOrWhiteSpace(applicationTableName))
                {
                    application.SetTableName(applicationTableName);
                }
            },
            authorization =>
            {
                authorization.SetSchema(schemaName);
                
                if (!string.IsNullOrWhiteSpace(authorizationTableName))
                {
                    authorization.SetTableName(authorizationTableName);
                }            
            },
            scope =>
            {
                scope.SetSchema(schemaName);
                
                if (!string.IsNullOrWhiteSpace(scopeTableName))
                {
                    scope.SetTableName(scopeTableName);
                }    
            },
            token =>
            {
                token.SetSchema(schemaName);
                
                if (!string.IsNullOrWhiteSpace(tokenTableName))
                {
                    token.SetTableName(tokenTableName);
                }    
            }
        );
    }

    public static void ConfigureOpenIddict(
        this ModelBuilder modelBuilder,
        Action<IMutableEntityType>? configureApplication = null,
        Action<IMutableEntityType>? configureAuthorization = null,
        Action<IMutableEntityType>? configureScope = null,
        Action<IMutableEntityType>? configureToken = null)
    {
        modelBuilder.ConfigureOpenIddict<
            OpenIddictEntityFrameworkCoreApplication,
            OpenIddictEntityFrameworkCoreAuthorization,
            OpenIddictEntityFrameworkCoreScope,
            OpenIddictEntityFrameworkCoreToken,
            string
        >(
            configureApplication,
            configureAuthorization,
            configureScope,
            configureToken
        );
    }
    
    public static void ConfigureOpenIddict<TApplication, TAuthorization, TScope, TToken, TKey>(
        this ModelBuilder modelBuilder,
        Action<IMutableEntityType>? configureApplication = null,
        Action<IMutableEntityType>? configureAuthorization = null,
        Action<IMutableEntityType>? configureScope = null,
        Action<IMutableEntityType>? configureToken = null)
        where TApplication : OpenIddictEntityFrameworkCoreApplication<TKey, TAuthorization, TToken>
        where TAuthorization : OpenIddictEntityFrameworkCoreAuthorization<TKey, TApplication, TToken>
        where TScope : OpenIddictEntityFrameworkCoreScope<TKey>
        where TToken : OpenIddictEntityFrameworkCoreToken<TKey, TApplication, TAuthorization>
        where TKey : IEquatable<TKey>
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            for (var type = entity; type != null; type = type.BaseType)
            {
                if (type.ClrType == typeof(TApplication))
                {
                    configureApplication?.Invoke(type);
                    break;
                }
                
                if (type.ClrType == typeof(TAuthorization))
                {
                    configureAuthorization?.Invoke(type);
                    break;
                }
                
                if (type.ClrType == typeof(TScope))
                {
                    configureScope?.Invoke(type);
                    break;
                }
                
                if (type.ClrType == typeof(TToken))
                {
                    configureToken?.Invoke(type);
                    break;
                }
            }
        }
    }
}