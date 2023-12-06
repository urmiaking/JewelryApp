using System.ComponentModel.DataAnnotations;
using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.CommonDtos;
using JewelryApp.Models.Dtos.ProductDtos;
using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.InvoiceDtos;

public class InvoiceItemDto : BaseDto<InvoiceItemDto, InvoiceItem>
{
    [Display(Name = "ردیف")]
    public int Index { get; set; } = 0;
    
    [Display(Name = "ضریب مالیات")]
    public double TaxOffset { get; set; } = 9;

    [Display(Name = "تعداد")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "سود")]
    public double Profit { get; set; } = 7;

    public ProductDto Product { get; set; } = new();

    public InvoiceDto Invoice { get; set; } = new();

    [JsonIgnore]
    public double Tax
    {
        get
        {
            if (Product is not null)
            {
                return Product.ProductType switch
                {
                    ProductType.Gold => Product.Weight * (TaxOffset / 100.0 * (Profit + Product.Wage) / 100) * Invoice.GramPrice,
                    ProductType.Jewelry => (Product.Wage + Invoice.GramPrice * Profit) * (TaxOffset / 100),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            return 0;
        }
    }

    [JsonIgnore]
    private double RawPrice
    {
        get
        {
            if (Product is not null)
            {
                return Product.ProductType switch
                {
                    ProductType.Gold => (Product.Weight + Product.Weight * Profit / 100.0 + (Product.Weight + Product.Weight * Profit / 100.0) * Product.Wage / 100.0) * Invoice.GramPrice,
                    ProductType.Jewelry => (Invoice.GramPrice + Product.Wage + Profit * Invoice.GramPrice) * Product.Weight,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            return 0;
        }
    }

    public double FinalPrice => RawPrice + Tax;
}