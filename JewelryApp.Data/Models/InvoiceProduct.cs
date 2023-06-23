namespace JewelryApp.Data.Models;

public class InvoiceProduct
{
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Count { get; set; }
    public double Profit { get; set; } // سود (درصدی) هستش مثلا 0.09

    public double Tax {get; set; } 

    public double TaxOffset { get; set; }

    public double Wage { get; set; } // اجرت جواهر به تومان است، اجرت طلا به درصد است
    public double GramPrice { get; set; }

    public double FinalPrice { get; set; }
}
