using JewelryApp.Data.Models;

namespace JewelryApp.Models.ViewModels;

public class InvoiceIndexVm
{
    public List<Invoice> Invoices { get; set; } = new();
}