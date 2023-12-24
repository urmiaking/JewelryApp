namespace JewelryApp.Shared.Responses.Invoices;

public record GetInvoiceResponse(int Id, int InvoiceNumber, int CustomerId, DateTime InvoiceDate, 
    double Discount, double AdditionalPrices, double Difference, double Debt, 
    DateTime? DebtDate, double TotalRawPrice, double TotalTax, double TotalFinalPrice);