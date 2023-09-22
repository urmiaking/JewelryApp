using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Data.Models;

public class Invoice
{
    public int Id { get; set; }

    [Display(Name = "نام خریدار")]
    public string BuyerFirstName { get; set; }

    [Display(Name = "نام خانوادگی خریدار")]
    public string BuyerLastName { get; set; }

    [Display(Name = "کد ملی خریدار")]
    public string BuyerNationalCode { get; set; }

    [Display(Name = "شماره همراه خریدار")]
    public string BuyerPhoneNumber { get; set; }
    public List<InvoiceProduct> InvoiceProducts { get; set; }

    [Display(Name = "تاریخ فاکتور")]
    public DateTime? BuyDateTime { get; set; }
}
