using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenIddict.EntityFrameworkCore.Models;

// ReSharper disable InvertIf

namespace Nixon.Extensions.OpenIddict.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void UseOpenIddict(
        this ModelBuilder modelBuilder,
        string schemaName,
        string applicationTableName,
        string authorizationTableName,
        string scopeTableName,
        string tokenTableName)
    {
        modelBuilder.UseOpenIddict(
            application =>
            {
                application.SetSchema(schemaName);
                application.SetTableName(applicationTableName);
            },
            authorization =>
            {
                authorization.SetSchema(schemaName);
                authorization.SetTableName(authorizationTableName); 
            },
            scope =>
            {
                scope.SetSchema(schemaName);
                scope.SetTableName(scopeTableName);
            },
            token =>
            {
                token.SetSchema(schemaName);
                token.SetTableName(tokenTableName);
            }
        );
    }

    public static void UseOpenIddict(
        this ModelBuilder modelBuilder,
        Action<IMutableEntityType>? configureApplication = null,
        Action<IMutableEntityType>? configureAuthorization = null,
        Action<IMutableEntityType>? configureScope = null,
        Action<IMutableEntityType>? configureToken = null)
    {
        modelBuilder.UseOpenIddict<
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
    
    public static void UseOpenIddict<TApplication, TAuthorization, TScope, TToken, TKey>(
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
        modelBuilder.UseOpenIddict();
        
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            for (var type = entity; type != null; type = type.BaseType)
            {
                if (type.ClrType.Assembly == typeof(TApplication).Assembly)
                {
                    configureApplication?.Invoke(type);
                    break;
                }
                
                if (type.ClrType.Assembly == typeof(TAuthorization).Assembly)
                {
                    configureAuthorization?.Invoke(type);
                    break;
                }
                
                if (type.ClrType.Assembly == typeof(TScope).Assembly)
                {
                    configureScope?.Invoke(type);
                    break;
                }
                
                if (type.ClrType.Assembly == typeof(TToken).Assembly)
                {
                    configureToken?.Invoke(type);
                    break;
                }
            }
        }
    }
}