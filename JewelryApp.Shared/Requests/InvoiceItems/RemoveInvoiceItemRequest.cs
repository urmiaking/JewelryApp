﻿namespace JewelryApp.Shared.Requests.InvoiceItems;

public record RemoveInvoiceItemRequest(int Id, int InvoiceId, int ProductId);