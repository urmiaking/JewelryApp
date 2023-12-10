namespace JewelryApp.Shared.Requests.InvoiceItems;

public record UpdateInvoiceItemRequest (int Id, int InvoiceId, int ProductId, int Quantity, double Profit, double GramPrice,
    double? DollarPrice, double TaxOffset, double Tax, double Price);