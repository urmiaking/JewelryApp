namespace JewelryApp.Shared.Requests.Invoices;

public record UpdateInvoiceRequest(int Id, int InvoiceNumber, DateTime InvoiceDate, double? Debt, DateTime? DebtDate, double? AdditionalPrices, double? Discount, int CustomerId);