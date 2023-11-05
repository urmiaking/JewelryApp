﻿using JewelryApp.Common.Enums;

namespace JewelryApp.Models.Dtos.Product;

public class ProductCalculationDto
{
    public double GramPrice { get; set; } = 0;
    public double Weight { get; set; } = 0;
    public double Wage { get; set; } = 0;
    public double Profit { get; set; } = 0;
    public double TaxOffset { get; set; } = 0;
    public string ProductName { get; set; } = string.Empty;
    public ProductType ProductType { get; set; } = ProductType.Gold;

    public double RawPrice
    {
        get
        {
            return ProductType switch
            {
                ProductType.Gold => (Weight + Weight * Profit / 100.0 + (Weight + Weight * Profit / 100.0) * Wage / 100.0) * GramPrice,
                ProductType.Jewelry => (GramPrice + Wage + Profit * GramPrice) * Weight,
                _ => throw new ArgumentOutOfRangeException()
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
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public double FinalPrice => RawPrice + Tax;

}