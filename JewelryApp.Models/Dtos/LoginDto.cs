using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Models.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
    [Display(Name = "نام کاربری")]
    [MaxLength(50, ErrorMessage = "لطفا حداکثر 50 کاراکتر وارد نمایید")]
    public string Username { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
    [Display(Name = "رمز عبور")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}