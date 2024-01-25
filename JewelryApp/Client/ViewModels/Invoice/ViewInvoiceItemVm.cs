using JewelryApp.Shared.Enums;

namespace JewelryApp.Client.ViewModels.Invoice;

public class ViewInvoiceItemVm
{
    public string Name { set; get; } = default!;
    public double Weight { set; get; }
    public double Wage { set; get; }
    public double Profit { set; get; } = 7;
    public double TaxOffset { get; set; } = 9;

    public double GramPrice { get; set; }
    public double DollarPrice { get; set; }

    public WageType WageType { set; get; } = default!;
    public ProductType ProductType { set; get; } = default!;
    public CaratType CaratType { set; get; }

    public string CategoryName { set; get; } = default!;
    public string Barcode { set; get; } = default!;

    public double Tax
    {
        get
        {
            switch (CaratType)
            {
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
                ProductType.Gold => Weight * (TaxOffset / 100.0 * (Profit + Wage) / 100) * GramPrice,
                ProductType.Jewelry => WageType switch
                {
                    WageType.Toman => (Wage * Weight + GramPrice * Weight) * (TaxOffset / 100.0),
                    WageType.Dollar => (DollarPrice * Wage / Weight * Weight + GramPrice * Weight) * (TaxOffset / 100.0),
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
                ProductType.Gold => (Weight + Weight * Profit / 100.0 + (Weight + Weight * Profit / 100.0) * Wage / 100.0) * GramPrice,
                ProductType.Jewelry => WageType switch
                {
                    WageType.Toman => (Wage + GramPrice + (Wage + GramPrice) * Profit / 100.0) * Weight,
                    WageType.Dollar => (DollarPrice * Wage / Weight + GramPrice + (DollarPrice * Wage / Weight + GramPrice) * Profit / 100.0) * Weight,
                    _ => 0
                },
                _ => 0
            };
        }
    }

    public double FinalPrice => Price + Tax;
}