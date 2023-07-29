using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Models.Dtos;

public class ApiKeyDto
{
    [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
    [Display(Name = "کلید API")]
    [MaxLength(50, ErrorMessage = "لطفا حداکثر 50 کاراکتر وارد نمایید")]
    public string Key { get; set; }

    [Display(Name = "ذخیره به عنوان کلید فعال")]
    public bool IsActive { get; set; }
}