using System.ComponentModel.DataAnnotations;
using JewelryApp.Common.Enums;

namespace JewelryApp.Models.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    [Display(Name = "ردیف")]
    public int Index { get; set; } = 0;

    [Display(Name = "نام جنس")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "عیار")]
    public Caret Caret { get; set; } = Caret.Eighteen;

    [Display(Name = "وزن")]
    public double Weight { get; set; }

    [Display(Name = "نوع")]
    public ProductType ProductType { get; set; } = ProductType.Gold;

    [Display(Name = "ضریب مالیات")]
    public double TaxOffset { get; set; } = 9;

    [Display(Name = "اجرت")]
    public double Wage { get; set; } // اجرت جواهر به تومان است، اجرت طلا به درصد است

    [Display(Name = "نرخ روز گرم")]
    public double GramPrice { get; set; }

    [Display(Name = "تعداد")]
    public int Count { get; set; } = 1;

    [Display(Name = "سود")]
    public double Profit { get; set; } = 7;

    public double Tax
    {
        get
        {
            return ProductType switch
            {
                ProductType.Gold => Weight * (((TaxOffset / 100.0) * (Profit + Wage)) / 100) * GramPrice,
                ProductType.Jewelry => (Wage + (GramPrice * Profit)) * (TaxOffset / 100),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    } // مالیات (Calculated)

    private double _finalPrice;

    private double Price // قیمت بدون مالیات
    {
        get
        {
            return ProductType switch
            {
                ProductType.Gold => (Weight + (Weight * Profit / 100.0) + (Weight + (Weight * Profit / 100.0)) * Wage / 100.0) * GramPrice,
                ProductType.Jewelry => (GramPrice + Wage + (Profit * GramPrice)) * Weight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public double FinalPrice // قیمت با مالیات
    {
        get => Price + Tax;
        set => _finalPrice = value;
    }
}