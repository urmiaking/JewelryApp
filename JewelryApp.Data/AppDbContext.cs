using JewelryApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }
    }

	public DbSet<Invoice> Invoices { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<InvoiceProduct> InvoiceProducts { get; set; }
	public DbSet<GramPrice> GramPrices { get; set; }
	public DbSet<ApiKey> ApiKeys { get; set; }
	public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InvoiceProduct>()
        .HasKey(ip => new { ip.InvoiceId, ip.ProductId });

        modelBuilder.Entity<InvoiceProduct>()
            .HasOne(ip => ip.Invoice)
            .WithMany(i => i.InvoiceProducts)
            .HasForeignKey(ip => ip.InvoiceId);

        modelBuilder.Entity<InvoiceProduct>()
            .HasOne(ip => ip.Product)
            .WithMany(p => p.InvoiceProducts)
            .HasForeignKey(ip => ip.ProductId);

        SetupIdentityTables(modelBuilder);
    }

    private void SetupIdentityTables(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(b =>
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
        builder.Entity<ApplicationRole>(b =>
        {
            b.ToTable("Roles");

            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });
        builder.Entity<ApplicationRoleClaim>(b => {
            b.ToTable("RoleClaims");

            b.HasOne(e => e.Role)
                .WithMany(e => e.Claims)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();

        });
        builder.Entity<ApplicationUserClaim>(b => {
            b.ToTable("UserClaims");

            b.HasOne(e => e.User)
                .WithMany(e => e.Claims)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();
        });
        builder.Entity<ApplicationUserLogin>(b => {
            b.ToTable("UserLogins");

            b.HasOne(e => e.User)
                .WithMany(e => e.Logins)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();
        });
        builder.Entity<ApplicationUserRole>(b => {
            b.ToTable("UserRoles");

            b.HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        builder.Entity<ApplicationUserToken>(b => {
            b.ToTable("UserTokens");

            b.HasOne(e => e.User)
                .WithMany(e => e.Tokens)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();
        });
    }
}
