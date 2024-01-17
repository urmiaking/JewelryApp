﻿using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Shared.Enums;

public enum WageType
{
    [Display(Name = "درصد")]
    Percent = 1,
    [Display(Name = "تومان")]
    Toman = 2,
    [Display(Name = "دلار")]
    Dollar = 3
}
