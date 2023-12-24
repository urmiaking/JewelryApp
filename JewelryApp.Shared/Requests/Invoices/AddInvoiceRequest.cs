namespace JewelryApp.Shared.Requests.Invoices;

public record AddInvoiceRequest(int InvoiceNumber, DateTime InvoiceDate, double? Debt, DateTime? DebtDate, double? AdditionalPrices, double? Discount, int CustomerId);
