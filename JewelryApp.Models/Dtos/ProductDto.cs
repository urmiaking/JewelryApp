using System.ComponentModel.DataAnnotations;
using JewelryApp.Common.Enums;

namespace JewelryApp.Models.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    [Display(Name = "شرح جنس")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "عیار")]
    public Caret Caret { get; set; }

    [Display(Name = "وزن")]
    public double Weight { get; set; }

    [Display(Name = "نوع")]
    public ProductType ProductType { get; set; }

    [Display(Name = "ضریب مالیات")]
    public double TaxOffset { get; set; } = 0.09;

    [Display(Name = "اجرت")]
    public double Wage { get; set; } // اجرت جواهر به تومان است، اجرت طلا به درصد است

    [Display(Name = "نرخ روز گرم")]
    public double GramPrice { get; set; }

    [Display(Name = "تعداد")]
    public int Count { get; set; }

    [Display(Name = "سود")]
    public double Profit { get; set; } // سود (درصدی) هستش مثلا 0.09

    public double Tax
    {
        get
        {
            return ProductType switch
            {
                ProductType.Gold => Weight * ((Wage * GramPrice) + (Profit * GramPrice) * (TaxOffset / 100) ),
                ProductType.Jewelry => Wage + (GramPrice * Profit) * (TaxOffset / 100),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    } // مالیات (Calculated)

    private double _finalPrice;

    private double Price
    {
        get
        {
            return ProductType switch
            {
                ProductType.Gold => ((GramPrice + (Wage * GramPrice) + ((Profit / 100) * GramPrice)) * Weight),
                ProductType.Jewelry => (GramPrice + Wage + ((Profit / 100) * GramPrice)) * Weight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public double FinalPrice
    {
        get => Price + Tax;
        set => _finalPrice = value;
    }
}