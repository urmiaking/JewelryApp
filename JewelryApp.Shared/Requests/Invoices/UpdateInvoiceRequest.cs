namespace JewelryApp.Shared.Requests.Invoices;

public record UpdateInvoiceRequest(int Id, DateTime InvoiceDate, double? Debt, DateTime? DebtDate, double? AdditionalPrices, double? Discount);