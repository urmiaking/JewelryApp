namespace JewelryApp.Core.DomainModels;

public class InvoiceItem : SoftDeleteModelBase
{
    public int Quantity { get; set; } = 1;
    public double Profit { get; set; }
    public double TaxOffset { get; set; }
    public double Tax { get; set; }
    public double Price { get; set; }
    public double? DollarPrice { get; set; }
    public double GramPrice { get; set; }

    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public Invoice Invoice { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
