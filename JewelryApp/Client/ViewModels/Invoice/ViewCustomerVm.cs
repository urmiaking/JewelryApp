using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels.Invoice;

public class ViewCustomerVm
{
    [Display(Name = "نام و نام خانوادگی مشتری")]
    public string Name { get; set; } = default!;

    [Display(Name = "شماره همراه مشتری")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "کد ملی مشتری")]
    public string? NationalCode { get; set; }
}