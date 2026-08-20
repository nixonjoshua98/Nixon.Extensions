using Microsoft.EntityFrameworkCore;
using Nixon.Extensions.OpenIddict.EntityFrameworkCore;

namespace Nixon.Extensions.OpenIddict.Samples.Alpha;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseOpenIddict();
        
        modelBuilder.ConfigureOpenIddict(
            "openiddict"
        );
    }
}