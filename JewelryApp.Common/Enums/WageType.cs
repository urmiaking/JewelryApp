using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Common.Enums;

public enum WageType
{
    [Display(Name = "جواهر")]
    Jewelry = 1,
    [Display(Name = "طلا")]
    Gold = 2
}
