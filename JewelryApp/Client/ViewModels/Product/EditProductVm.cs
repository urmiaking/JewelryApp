﻿using JewelryApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels.Product;

public class EditProductVm
{
    public int Id { set; get; }

    [Display(Name = "نام جنس")]
    [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
    public string Name { set; get; } = default!;

    [Display(Name = "وزن")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Weight { set; get; }

    [Display(Name = "اجرت")]
    [Range(0, 100, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Wage { set; get; }

    [Display(Name = "نوع اجرت")]
    public WageType WageType { set; get; } = default!;

    [Display(Name = "نوع جنس")]
    public ProductType ProductType { set; get; } = default!;

    [Display(Name = "عیار")]
    public CaratType CaratType { set; get; }

    [Display(Name = "دسته بندی")]
    public ProductCategoryVm ProductCategory { set; get; } = new() { Id = 0, Name = "انتخاب کنید" };

    public string CategoryName { get; set; } = default!;

    [Display(Name = "بارکد")]
    public string Barcode { set; get; } = default!;

    public List<ProductCategoryVm> ProductCategories { get; set; } = new();
}