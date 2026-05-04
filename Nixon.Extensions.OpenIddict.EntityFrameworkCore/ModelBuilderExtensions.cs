using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenIddict.EntityFrameworkCore.Models;

namespace Nixon.Extensions.OpenIddict.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void ConfigureDefaultOpenIddictModels(
        this ModelBuilder modelBuilder, 
        Action<IMutableEntityType> configure)
    {
        var modelAssembly = typeof(OpenIddictEntityFrameworkCoreApplication).Assembly;
        
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            for (var type = entity; type != null; type = type.BaseType)
            {
                if (type.ClrType.Assembly != modelAssembly)
                {
                    continue;
                }
                
                configure(type);

                break;
            }
        }
    }
}