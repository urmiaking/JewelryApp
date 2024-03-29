﻿using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels.Invoice;

public class ViewInvoiceVm
{
    [Display(Name = "شماره فاکتور")]
    public int InvoiceNumber { get; set; }

    [Display(Name = "تاریخ فاکتور")]
    public DateTime? BuyDateTime { get; set; } = DateTime.Now;

    [Display(Name = "تخفیف")]
    public double Discount { get; set; }

    [Display(Name = "هزینه های اضافی")]
    public double AdditionalPrices { get; set; }

    [Display(Name = "بدهی")]
    public double Debt { get; set; }

    [Display(Name = "موعد بدهی")]
    public DateTime? DebtDate { get; set; } = DateTime.Now;
}