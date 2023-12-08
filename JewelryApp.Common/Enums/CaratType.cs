using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Common.Enums;

public enum CaratType
{
    [Display(Name = "17 عیار")]
    SevenTeen = 0,

    [Display(Name = "18 عیار")]
    Eighteen = 1,

    [Display(Name = "21 عیار")]
    TwentyOne = 2,

    [Display(Name = "22 عیار")]
    TwentyTwo = 3,

    [Display(Name = "24 عیار")]
    TwentyFour = 4
}