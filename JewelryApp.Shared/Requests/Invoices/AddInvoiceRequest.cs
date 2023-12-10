namespace JewelryApp.Shared.Requests.Invoices;

public record AddInvoiceRequest( DateTime InvoiceDate, double? Debt, DateTime? DebtDate, double? AdditionalPrices, double? Discount);
