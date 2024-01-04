using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Shared.Enums;

public enum CalculationProductType
{
    [Display(Name = "جواهر")]
    Jewelry = 1,
    [Display(Name = "طلا")]
    Gold = 2,
    [Display(Name = "طلای کهنه")]
    OldGold = 3
}
