﻿using System.ComponentModel.DataAnnotations;
using JewelryApp.Client.ViewModels.Populated;
using JewelryApp.Shared.Enums;

namespace JewelryApp.Client.ViewModels;

public class ProductListVm
{
    public int Id { get; set; }

    [Display(Name = "نام جنس")]
    public string Name { get; set; } = default!;

    [Display(Name = "وزن")]
    public double Weight { get; set; }

    [Display(Name = "اجرت")]
    public double Wage { get; set; }

    [Display(Name = "نوع اجرت")]
    public WageType WageType { get; set; } = default!;

    [Display(Name = "نوع کالا")]
    public ProductType ProductType { get; set; } = default!;

    [Display(Name = "عیار")]
    public CaratType CaratType { get; set; } = default!;

    [Display(Name = "دسته بندی")]
    public string CategoryName { get; set; } = default!;

    [Display(Name = "بارکد")]
    public string Barcode { get; set; } = default!;
    public bool Deleted { get; set; }

    [Display(Name = "دسته بندی")]
    public ProductCategoryVm ProductCategory { set; get; } = new() { Id = 0, Name = "انتخاب کنید" };

    public List<ProductCategoryVm> ProductCategories { get; set; } = new();
}