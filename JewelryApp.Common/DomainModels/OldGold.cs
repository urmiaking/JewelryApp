namespace JewelryApp.Core.DomainModels;

public class OldGold : SoftDeleteModelBase
{
    public string Name { get; set; } = default!;
    public double Weight { get; set; }
    public DateTime? BuyDateTime { get; set; }
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = default!;
    public double Price { get; set; }
}