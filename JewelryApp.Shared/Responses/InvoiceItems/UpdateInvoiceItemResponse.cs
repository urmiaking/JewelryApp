namespace JewelryApp.Shared.Responses.InvoiceItems;

public record UpdateInvoiceItemResponse(int Id, int ProductId, int InvoiceId,  int Quantity, double Profit, double GramPrice,
    double? DollarPrice, double TaxOffset, double Tax, double Price);