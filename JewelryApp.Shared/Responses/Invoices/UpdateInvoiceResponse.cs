namespace JewelryApp.Shared.Responses.Invoices;

public record UpdateInvoiceResponse(int Id, int InvoiceNumber, int CustomerId, DateTime InvoiceDate,
    double Discount, double AdditionalPrices, double Difference, double Debt, DateTime? DebtDated);