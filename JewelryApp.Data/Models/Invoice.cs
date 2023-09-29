using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Data.Models;

public class Invoice
{
    public int Id { get; set; }

    public string BuyerFirstName { get; set; }

    public string BuyerLastName { get; set; }

    public string BuyerNationalCode { get; set; }

    public string BuyerPhoneNumber { get; set; }
    public List<InvoiceProduct> InvoiceProducts { get; set; }

    public DateTime? BuyDateTime { get; set; }

    public double Discount { get; set; } = 0;

    public double Debt { get; set; } = 0;

    public DateTime? DebtDate { get; set; }
}
