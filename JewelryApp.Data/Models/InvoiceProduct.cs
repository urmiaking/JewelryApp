namespace JewelryApp.Data.Models;

public class InvoiceProduct
{
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Count { get; set; }
    public double Profit { get; set; }

    public double Tax { get; set; }

    public double TaxOffset { get; set; }

    public double GramPrice { get; set; }

    public double FinalPrice { get; set; }
}
