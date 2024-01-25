using JewelryApp.Core.Enums;
using JewelryApp.Shared.Enums;

namespace JewelryApp.Core.DomainModels;

public class InvoiceItem : SoftDeleteModelBase
{
    public double Profit { get; set; }
    public double TaxOffset { get; set; }
    public double Tax { get; set; }
    public double Price { get; set; }
    public double? DollarPrice { get; set; }
    public double GramPrice { get; set; }

    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public Invoice Invoice { get; set; } = null!;
    public Product Product { get; set; } = null!;

    public double CalculateRawPrice()
    {
        switch (Product.Carat)
        {
            case CaratType.Eighteen:
                break;
            case CaratType.TwentyOne:
                Product.Weight = Product.Weight * 21.0 / 18.0;
                break;
            case CaratType.TwentyTwo:
                Product.Weight = Product.Weight * 22.0 / 18.0;
                break;
        }

        var tax = Product.ProductType switch
        {
            ProductType.Gold => Product.Weight * (TaxOffset / 100.0 * (Profit + Product.Wage) / 100) * GramPrice,
            ProductType.Jewelry => Product.WageType switch
            {
                WageType.Toman => (Product.Wage * Product.Weight + GramPrice * Product.Weight) * (TaxOffset / 100.0),
                WageType.Dollar => (DollarPrice * Product.Wage / Product.Weight * Product.Weight + GramPrice * Product.Weight) * (TaxOffset / 100.0),
                _ => 0
            },
            _ => 0
        };

        var price = Product.ProductType switch
        {
            ProductType.Gold => (Product.Weight + Product.Weight * Profit / 100.0 + (Product.Weight + Product.Weight * Profit / 100.0) * Product.Wage / 100.0) * GramPrice,
            ProductType.Jewelry => Product.WageType switch
            {
                WageType.Toman => (Product.Wage + GramPrice + (Product.Wage + GramPrice) * Profit / 100.0) * Product.Weight,
                WageType.Dollar => (DollarPrice * Product.Wage / Product.Weight + GramPrice + (DollarPrice * Product.Wage / Product.Weight + GramPrice) * Profit / 100.0) * Product.Weight,
                _ => 0
            },
            _ => 0
        };

        return tax + price ?? 0;
    }
}
