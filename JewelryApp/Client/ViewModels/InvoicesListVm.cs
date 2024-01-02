using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels;

public class InvoicesListVm
{
    public int Id { get; set; }

    [Display(Name = "شماره فاکتور")]
    public int InvoiceNumber { get; set; }

    [Display(Name = "نام مشتری")]
    public string CustomerName { get; set; } = default!;

    [Display(Name = "شماره همراه")]
    public string CustomerPhoneNumber { get; set; } = default!;

    [Display(Name = "مبلغ کل")]
    public double TotalCost { get; set; }

    [Display(Name = "تعداد اقلام")]
    public int InvoiceItemsCount { get; set; }

    [Display(Name = "تاریخ فاکتور")]
    public DateTime InvoiceDate { get; set; }
    public bool Deleted { get; set; }
}