using JewelryApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels;

public class CalculatorVm
{
    [Display(Name = "نام جنس")]
    public string Name { set; get; } = default!;

    [Display(Name = "نرخ گرم")]
    public double GramPrice { set; get; } = default!;

    [Display(Name = "نرخ دلار")]
    public double DollarPrice { set; get; } = default!;

    [Display(Name = "وزن")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Weight { set; get; }

    [Display(Name = "اجرت")]
    [Range(0, 100, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Wage { set; get; }

    [Display(Name = "مالیات")]
    [Range(0, 100, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double TaxOffset { set; get; }

    [Display(Name = "سود")]
    [Range(0, 100, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Profit { set; get; }

    [Display(Name = "نوع اجرت")]
    public WageType WageType { set; get; }

    [Display(Name = "نوع جنس")]
    public CalculationProductType ProductType { set; get; }

    [Display(Name = "عیار")]
    public CaratType CaratType { set; get; }

    [Display(Name = "دسته بندی")]
    public int CategoryId { set; get; }

    [Display(Name = "بارکد")]
    public string Barcode { set; get; } = default!;

    public List<ProductCategoryVm> ProductCategories { get; set; } = new();

    public double Tax
    {
        get
        {
            switch (CaratType)
            {
                case CaratType.SevenTeen:
                    Weight = Weight * 17.0 / 18.0;
                    break;
                case CaratType.Eighteen:
                    break;
                case CaratType.TwentyOne:
                    Weight = Weight * 21.0 / 18.0;
                    break;
                case CaratType.TwentyTwo:
                    Weight = Weight * 22.0 / 18.0;
                    break;
            }

            return ProductType switch
            {
                CalculationProductType.Gold => Weight * (((TaxOffset / 100.0) * (Profit + Wage)) / 100) * GramPrice,
                CalculationProductType.Jewelry => WageType switch
                {
                    WageType.Toman => ((Wage * Weight) + (GramPrice * Weight)) * (TaxOffset / 100.0),
                    WageType.Dollar => ((((DollarPrice * Wage) / Weight) * Weight) + (GramPrice * Weight)) * (TaxOffset / 100.0),
                    _ => 0
                },
                _ => 0
            };
        }
    }

    public double Price
    {
        get
        {
            switch (CaratType)
            {
                case CaratType.SevenTeen:
                    Weight = Weight * 17.0 / 18.0;
                    break;
                case CaratType.Eighteen:
                    break;
                case CaratType.TwentyOne:
                    Weight = Weight * 21.0 / 18.0;
                    break;
                case CaratType.TwentyTwo:
                    Weight = Weight * 22.0 / 18.0;
                    break;
            }

            return ProductType switch
            {
                CalculationProductType.Gold => (Weight + (Weight * Profit / 100.0) + (Weight + (Weight * Profit / 100.0)) * Wage / 100.0) * GramPrice,
                CalculationProductType.Jewelry => WageType switch
                {   
                    WageType.Toman => ((Wage + GramPrice) + ((Wage + GramPrice) * Profit / 100.0)) * Weight,
                    WageType.Dollar => ((((DollarPrice * Wage) / Weight) + GramPrice) + ((((DollarPrice * Wage) / Weight) + GramPrice) * Profit / 100.0)) * Weight,
                    _ => 0
                },
                _ => 0
            };
        }
    }

    public double FinalPrice => Tax + Price;
}