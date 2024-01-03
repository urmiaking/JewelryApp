using JewelryApp.Client.ViewModels.Populated;
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
    public int WageType { set; get; }

    [Display(Name = "نوع جنس")]
    public int ProductType { set; get; }

    [Display(Name = "عیار")]
    public int CaratType { set; get; }

    [Display(Name = "دسته بندی")]
    public int CategoryId { set; get; }

    [Display(Name = "بارکد")]
    public string Barcode { set; get; } = default!;

    public ProductTypeVm ProductTypeVm { get; set; } = new();
    public CaratTypeVm CaratTypeVm { get; set; } = new();
    public WageTypeVm WageTypeVm { get; set; } = new();

    public List<ProductCategoryVm> ProductCategories { get; set; } = new();

    public double Tax
    {
        get
        {
            switch (CaratType)
            {
                case (int)JewelryApp.Shared.Enums.CaratType.SevenTeen:
                    Weight = Weight * 17.0 / 18.0;
                    break;
                case (int)JewelryApp.Shared.Enums.CaratType.Eighteen:
                    break;
                case (int)JewelryApp.Shared.Enums.CaratType.TwentyOne:
                    Weight = Weight * 21.0 / 18.0;
                    break;
                case (int)JewelryApp.Shared.Enums.CaratType.TwentyTwo:
                    Weight = Weight * 22.0 / 18.0;
                    break;
            }

            return ProductType switch
            {
                (int)JewelryApp.Shared.Enums.ProductType.Gold => Weight * (((TaxOffset / 100.0) * (Profit + Wage)) / 100) * GramPrice,
                (int)JewelryApp.Shared.Enums.ProductType.Jewelry => WageType switch
                {
                    (int)JewelryApp.Shared.Enums.WageType.Toman => ((Wage * Weight) + (GramPrice * Weight)) * (TaxOffset / 100.0),
                    (int)JewelryApp.Shared.Enums.WageType.Dollar => ((((DollarPrice * Wage) / Weight) * Weight) + (GramPrice * Weight)) * (TaxOffset / 100.0),
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
                case (int)JewelryApp.Shared.Enums.CaratType.SevenTeen:
                    Weight = Weight * 17.0 / 18.0;
                    break;
                case (int)JewelryApp.Shared.Enums.CaratType.Eighteen:
                    break;
                case (int)JewelryApp.Shared.Enums.CaratType.TwentyOne:
                    Weight = Weight * 21.0 / 18.0;
                    break;
                case (int)JewelryApp.Shared.Enums.CaratType.TwentyTwo:
                    Weight = Weight * 22.0 / 18.0;
                    break;
            }

            return ProductType switch
            {
                (int)JewelryApp.Shared.Enums.ProductType.Gold => (Weight + (Weight * Profit / 100.0) + (Weight + (Weight * Profit / 100.0)) * Wage / 100.0) * GramPrice,
                (int)JewelryApp.Shared.Enums.ProductType.Jewelry => WageType switch
                {
                    (int)JewelryApp.Shared.Enums.WageType.Toman => ((Wage + GramPrice) + ((Wage + GramPrice) * Profit / 100.0)) * Weight,
                    (int)JewelryApp.Shared.Enums.WageType.Dollar => ((((DollarPrice * Wage) / Weight) + GramPrice) + ((((DollarPrice * Wage) / Weight) + GramPrice) * Profit / 100.0)) * Weight,
                    _ => 0
                },
                _ => 0
            };
        }
    }

    public double FinalPrice => Tax + Price;
}