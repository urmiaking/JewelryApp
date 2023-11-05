using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Common.Enums;

public enum WageType
{
    [Display(Name = "درصدی")]
    Percent = 1,
    [Display(Name = "تومان")]
    Toman = 2,
    [Display(Name = "دلاری")]
    Dollar = 3
}
