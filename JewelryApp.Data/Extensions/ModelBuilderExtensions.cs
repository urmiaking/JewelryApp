using System.Reflection;
using JewelryApp.Core.DomainModels.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JewelryApp.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Set NEWSEQUENTIALID() sql function for all columns named "Id"
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddSequentialGuidForIdConvention(this ModelBuilder modelBuilder)
    {
        modelBuilder.AddDefaultValueSqlConvention("Id", typeof(Guid), "NEWSEQUENTIALID()");
    }

    /// <summary>
    /// Set DefaultValueSql for specific property name and type
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="propertyName">Name of property wants to set DefaultValueSql for</param>
    /// <param name="propertyType">Type of property wants to set DefaultValueSql for </param>
    /// <param name="defaultValueSql">DefaultValueSql like "NEWSEQUENTIALID()"</param>
    public static void AddDefaultValueSqlConvention(this ModelBuilder modelBuilder, string propertyName, Type propertyType, string defaultValueSql)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var property = entityType.GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            if (property != null && property.ClrType == propertyType)
                property.SetDefaultValueSql(defaultValueSql);
        }
    }

    /// <summary>
    /// Set DeleteBehavior.Restrict by default for relations
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddRestrictDeleteBehaviorConvention(this ModelBuilder modelBuilder)
    {
        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => fk is { IsOwnership: false, DeleteBehavior: DeleteBehavior.Cascade });
        foreach (IMutableForeignKey fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;
    }

    /// <summary>
    /// Dynamically load all IEntityTypeConfiguration with Reflection
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterEntityTypeConfiguration(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        var applyGenericMethod = typeof(ModelBuilder).GetMethods().First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

        var types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c is { IsClass: true, IsAbstract: false, IsPublic: true });

        foreach (var type in types)
        {
            foreach (var iFace in type.GetInterfaces())
            {
                if (!iFace.IsConstructedGenericType ||
                    iFace.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>)) continue;
                var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iFace.GenericTypeArguments[0]);
                applyConcreteMethod.Invoke(modelBuilder, new[] { Activator.CreateInstance(type) });
            }
        }
    }

    /// <summary>
    /// Dynamically register all Entities that inherit from specific BaseType
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterAllEntities<TBaseType>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c is { IsClass: true, IsAbstract: false, IsPublic: true } && typeof(TBaseType).IsAssignableFrom(c));

        foreach (var type in types)
            modelBuilder.Entity(type);
    }

    /// <summary>
    /// Add Identity tables
    /// </summary>
    /// <param name="builder"></param>
    public static void SetupIdentityTables(this ModelBuilder builder)
    {
        builder.Entity<AppUser>(b =>
        {
            b.ToTable("Users");

            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        builder.Entity<AppRole>(b =>
        {
            b.ToTable("Roles");

            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });
        builder.Entity<AppRoleClaim>(b => {
            b.ToTable("RoleClaims");

            b.HasOne(e => e.Role)
                .WithMany(e => e.Claims)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();

        });
        builder.Entity<AppUserClaim>(b => {
            b.ToTable("UserClaims");

            b.HasOne(e => e.User)
                .WithMany(e => e.Claims)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();
        });
        builder.Entity<AppUserLogin>(b => {
            b.ToTable("UserLogins");

            b.HasOne(e => e.User)
                .WithMany(e => e.Logins)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();
        });
        builder.Entity<AppUserRole>(b => {
            b.ToTable("UserRoles");

            b.HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        builder.Entity<AppUserToken>(b => {
            b.ToTable("UserTokens");

            b.HasOne(e => e.User)
                .WithMany(e => e.Tokens)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();
        });
    }
}
