namespace JewelryApp.Client.ViewModels;

public class InvoicesListVm
{
    public int Id { get; set; }
    public int InvoiceNumber { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerPhoneNumber { get; set; } = default!;
    public double TotalCost { get; set; }
    public int InvoiceItemsCount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public bool Deleted { get; set; }
}