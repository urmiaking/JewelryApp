﻿using System.ComponentModel.DataAnnotations;
using JewelryApp.Common.Enums;

namespace JewelryApp.Data.Models;

public class Product
{
    public int Id { get; set; }

    [Display(Name = "نام جنس")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "عیار")]
    public Caret Caret { get; set; }

    [Display(Name = "وزن")]
    public double Weight { get; set; }

    [Display(Name = "اجرت")]
    public double Wage { get; set; } // اجرت جواهر به تومان است، اجرت طلا به درصد است

    public string BarcodeText { get; set; }

    [Display(Name = "نوع")]
    public ProductType ProductType { get; set; }

    public DateTime AddedDateTime { get; set; }

    public List<InvoiceProduct> InvoiceProducts { get; set; } = new();
}