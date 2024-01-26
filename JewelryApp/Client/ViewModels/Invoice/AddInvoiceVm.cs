using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels.Invoice;

public class AddInvoiceVm
{
    [Display(Name = "شماره فاکتور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public int InvoiceNumber { get; set; }

    [Display(Name = "تاریخ فاکتور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public DateTime? BuyDateTime { get; set; } = DateTime.Now;

    [Display(Name = "تخفیف")] 
    public double Discount { get; set; }

    [Display(Name = "هزینه های اضافی")]
    public double AdditionalPrices { get; set; }

    [Display(Name = "بدهی")]
    public double Debt { get; set; }

    [Display(Name = "موعد بدهی")]
    public DateTime? DebtDate { get; set; }

    public int CustomerId { get; set; }
}