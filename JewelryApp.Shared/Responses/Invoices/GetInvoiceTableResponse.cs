namespace JewelryApp.Shared.Responses.Invoices;

public record GetInvoiceTableResponse(int Id, string CustomerName, string CustomerPhoneNumber, double TotalCost, int InvoiceItemsCount, DateTime InvoiceDate);
