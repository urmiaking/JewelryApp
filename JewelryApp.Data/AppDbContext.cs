using JewelryApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
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

        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
    }
}
