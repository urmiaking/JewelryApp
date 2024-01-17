﻿using JewelryApp.Shared.Enums;

namespace JewelryApp.Client.ViewModels.Invoice;

public class AddInvoiceItemVm
{
    public int Id { set; get; }
    public string Name { set; get; } = default!;
    public double Weight { set; get; }
    public double Wage { set; get; }
    public double Profit { set; get; } = 7;
    public double TaxOffset { get; set; } = 9;
    public double Tax { get; set; }
    public double GramPrice { get; set; }
    public double DollarPrice { get; set; }
    public double FinalPrice { get; set; }
    public WageType WageType { set; get; } = default!;
    public ProductType ProductType { set; get; } = default!;
    public CaratType CaratType { set; get; }
    public string CategoryName { set; get; } = default!;
    public string Barcode { set; get; } = default!;
    public bool Deleted { set; get; }
}