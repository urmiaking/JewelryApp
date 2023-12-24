namespace JewelryApp.Shared.Responses.Invoices;

public record GetInvoiceListResponse(int Id, int InvoiceNumber, string CustomerName, string CustomerPhoneNumber, double TotalCost,
    int InvoiceItemsCount, DateTime InvoiceDate, bool Deleted);
