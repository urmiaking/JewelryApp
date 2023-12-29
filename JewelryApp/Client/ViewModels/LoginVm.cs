using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels;

public class LoginVm
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string UserName { get; set; } = default!;

    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MinLength(6, ErrorMessage = "{0} باید حداقل {1} رقم باشد.")]
    public string Password { get; set; } = default!;
}