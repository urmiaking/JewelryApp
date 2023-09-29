using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Data.Models;

public class Invoice
{
    public int Id { get; set; }
    public DateTime BuyDateTime { get; set; }
    public double Discount { get; set; } = 0;
    public double Debt { get; set; } = 0;
    public DateTime? DebtDate { get; set; }
    public double GramPrice { get; set; }
    public int CustomerId { get; set; }

    public Customer Customer { get; set; }
    public List<InvoiceItem> InvoiceItems { get; set; }
}
