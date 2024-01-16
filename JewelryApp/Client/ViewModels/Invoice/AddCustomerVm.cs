using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels.Invoice;

public class AddCustomerVm
{
    [Display(Name = "نام و نام خانوادگی مشتری")]
    [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
    public string Name { get; set; } = default!;

    [Display(Name = "شماره همراه مشتری")]
    public string? PhoneNumber { get; set; }
}