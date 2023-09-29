namespace JewelryApp.Data.Models;

public class InvoiceItem
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double Profit { get; set; }
    public double TaxOffset { get; set; }
    
    public Invoice Invoice { get; set; }
    public Product Product { get; set; }
}
