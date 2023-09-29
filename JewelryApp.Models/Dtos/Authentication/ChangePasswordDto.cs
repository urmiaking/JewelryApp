using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryApp.Models.Dtos.Authentication;

public class ChangePasswordDto
{
    [Display(Name = "رمز عبور فعلی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }

    [Display(Name = "رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Compare(nameof(NewPassword), ErrorMessage = "{0} با {1} مطابقت ندارد")]
    [Display(Name = "تایید رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}

