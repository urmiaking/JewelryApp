﻿using JewelryApp.Common.Enums;
using JewelryApp.Models.Dtos.CommonDtos;
using System.ComponentModel.DataAnnotations;
using JewelryApp.Data.Models;

namespace JewelryApp.Models.Dtos.ProductDtos;

public class ProductDto : BaseDto<ProductDto, Product>
{
    [Display(Name = "نام جنس")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "عیار")]
    public Carat Carat { get; set; } = Carat.Eighteen;

    [Display(Name = "وزن")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public double Weight { get; set; }

    [Display(Name = "نوع")]
    public ProductType ProductType { get; set; } = ProductType.Gold;

    [Display(Name = "اجرت")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public double Wage { get; set; }

    [Display(Name = "اجرت")]
    public WageType WageType { get; set; } = WageType.Percent;

    public string Barcode { get; set; } = string.Empty;
}