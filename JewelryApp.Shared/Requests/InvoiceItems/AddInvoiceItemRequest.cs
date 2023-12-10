﻿namespace JewelryApp.Shared.Requests.InvoiceItems;

public record AddInvoiceItemRequest(int InvoiceId, int ProductId, int Quantity, double Profit, double GramPrice,
    double? DollarPrice, double TaxOffset, double Tax, double Price);