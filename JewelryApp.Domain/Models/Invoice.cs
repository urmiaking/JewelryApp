namespace JewelryApp.Data.Models;

public class Invoice : SoftDeleteModelBase
{
    public double GramPrice { get; set; }
    public double Discount { get; set; }
    public double Debt { get; set; }
    public double AdditionalPrices { get; set; }
    public double TotalPrice { get; set; }
    public double Difference { get; set; }
    public double DollarPrice { get; set; }

    public DateTime InvoiceDate { get; set; }
    public DateTime? DebtDate { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    public virtual ICollection<OldGold>? OldGolds { get; set; }
}
