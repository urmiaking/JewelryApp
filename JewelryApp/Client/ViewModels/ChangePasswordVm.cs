using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels;

public class ChangePasswordVm
{
    public string? UserName { get; set; } = null!;

    [Display(Name = "رمز عبور قبلی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MinLength(6, ErrorMessage = "{0} باید حداقل {1} رقم باشد.")]
    public string OldPassword { get; set; } = null!;

    [Display(Name = "رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MinLength(6, ErrorMessage = "{0} باید حداقل {1} رقم باشد.")]
    public string NewPassword { get; set; } = null!;

    [Display(Name = "تکرار رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MinLength(6, ErrorMessage = "{0} باید حداقل {1} رقم باشد.")]
    public string ConfirmNewPassword { get; set; } = null!;
}