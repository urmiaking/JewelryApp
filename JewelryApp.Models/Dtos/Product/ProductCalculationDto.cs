using JewelryApp.Common.Enums;

namespace JewelryApp.Models.Dtos.Product;

public class ProductCalculationDto
{
    public double GramPrice { get; set; }
    public double Weight { get; set; }
    public double Wage { get; set; }
    public double Profit { get; set; }
    public double TaxOffset { get; set; }
    public string ProductName { get; set; }
    public ProductType ProductType { get; set; }

    public double RawPrice
    {
        get
        {
            return ProductType switch
            {
                ProductType.Gold => (Weight + Weight * Profit / 100.0 + (Weight + Weight * Profit / 100.0) * Wage / 100.0) * GramPrice,
                ProductType.Jewelry => (GramPrice + Wage + Profit * GramPrice) * Weight,
                _ => 0
            };
        }
    }

    public double Tax
    {
        get
        {
            return ProductType switch
            {
                ProductType.Gold => Weight * (TaxOffset / 100.0 * (Profit + Wage) / 100) * GramPrice,
                ProductType.Jewelry => (Wage + GramPrice * Profit) * (TaxOffset / 100),
                _ => 0
            };
        }
    }

    public double FinalPrice => RawPrice + Tax;

}