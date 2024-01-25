namespace JewelryApp.Client.ViewModels.Invoice;

public class AddOldGoldVm
{
    public string Name { set; get; } = default!;
    public double Weight { set; get; }
    public double GramPrice { get; set; }
    public DateTime BuyDateTime { get; set; } = DateTime.Now;
    public double Price => Weight * 735 / 750 * GramPrice;
    public int InvoiceId { get; set; }
}