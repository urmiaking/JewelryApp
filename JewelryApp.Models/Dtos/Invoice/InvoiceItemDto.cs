using System.ComponentModel.DataAnnotations;
using JewelryApp.Common.Enums;
using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Invoice;

public class InvoiceItemDto
{
    public int Id { get; set; }

    [Display(Name = "ردیف")]
    public int Index { get; set; } = 0;
    
    [Display(Name = "ضریب مالیات")]
    public double TaxOffset { get; set; } = 9;

    [Display(Name = "تعداد")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "سود")]
    public double Profit { get; set; } = 7;

    public Product.ProductDto Product { get; set; }
    public InvoiceDto Invoice { get; set; }

    [JsonIgnore]
    public double Tax
    {
        get
        {
            return Product.ProductType switch
            {
                ProductType.Gold => Product.Weight * (TaxOffset / 100.0 * (Profit + Product.Wage) / 100) * Invoice.GramPrice,
                ProductType.Jewelry => (Product.Wage + Invoice.GramPrice * Profit) * (TaxOffset / 100),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    [JsonIgnore]
    private double RawPrice
    {
        get
        {
            return Product.ProductType switch
            {
                ProductType.Gold => (Product.Weight + Product.Weight * Profit / 100.0 + (Product.Weight + Product.Weight * Profit / 100.0) * Product.Wage / 100.0) * Invoice.GramPrice,
                ProductType.Jewelry => (Invoice.GramPrice + Product.Wage + Profit * Invoice.GramPrice) * Product.Weight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public double FinalPrice => RawPrice + Tax;
}