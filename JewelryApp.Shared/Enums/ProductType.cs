using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Shared.Enums;

public enum ProductType
{
    [Display(Name = "جواهر")]
    Jewelry = 1,
    [Display(Name = "طلا")]
    Gold = 2
}