using System.Reflection;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Utilities;
using JewelryApp.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
       
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Must be called first of all (if using identity)
        base.OnModelCreating(builder);

        var entitiesAssembly = typeof(IEntity).Assembly;

        builder.RegisterAllEntities<IEntity>(entitiesAssembly);
        builder.RegisterEntityTypeConfiguration(entitiesAssembly);
        builder.SetupIdentityTables();
        builder.AddRestrictDeleteBehaviorConvention();
        builder.AddSequentialGuidForIdConvention();
    }

    public override int SaveChanges()
    {
        CleanString();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        CleanString();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        CleanString();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CleanString();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void CleanString()
    {
        var changedEntities = ChangeTracker.Entries()
            .Where(x => x.State is EntityState.Added or EntityState.Modified);
        foreach (var item in changedEntities)
        {
            var properties = item.Entity.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p is { CanRead: true, CanWrite: true } && p.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                var val = (string)property.GetValue(item.Entity, null)!;

                if (val.HasValue())
                {
                    var newVal = val.Fa2En().FixPersianChars();
                    if (newVal == val)
                        continue;
                    property.SetValue(item.Entity, newVal, null);
                }
            }
        }
    }
}
