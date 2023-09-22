﻿namespace JewelryApp.Models.Dtos;

public class InvoiceTableItemDto
{
    public int InvoiceId { get; set; }
    public string BuyerName { get; set; }
    public string BuyerPhone { get; set; }
    public double TotalCost { get; set; }
    public int ProductsCount { get; set; }
    public string BuyDate { get; set; }
}