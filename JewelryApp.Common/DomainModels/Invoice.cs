using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JewelryApp.Core.DomainModels;

public class Invoice : SoftDeleteModelBase
{
    public double Discount { get; set; }
    public double Debt { get; set; }
    public double AdditionalPrices { get; set; }
    public double Difference { get; set; }
    
    public DateTime InvoiceDate { get; set; }
    public DateTime? DebtDate { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    public virtual ICollection<OldGold> OldGolds { get; set; } = new List<OldGold>();
}

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder
           .HasOne(a => a.Customer)
           .WithMany(a => a.Invoices);
    }
}