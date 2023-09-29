namespace JewelryApp.Models.Dtos.Invoice;

public class InvoiceTableItemDto
{
    public int InvoiceId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public double TotalCost { get; set; }
    public int ProductsCount { get; set; }
    public string BuyDate { get; set; }
}