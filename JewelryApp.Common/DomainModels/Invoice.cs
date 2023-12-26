using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JewelryApp.Core.DomainModels;

public class Invoice : SoftDeleteModelBase
{
    public int InvoiceNumber { get; set; }
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

    public double CalculateTotalPrice()
    {
        var itemsPrice = InvoiceItems.Sum(x => x.Price);
        var oldGoldsPrice = OldGolds.Sum(x => x.Price);
        var tax = InvoiceItems.Sum(x => x.Tax);

        // TODO: figure out differnece
        return itemsPrice - oldGoldsPrice - Discount + AdditionalPrices - Debt + tax;
    }
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