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
}