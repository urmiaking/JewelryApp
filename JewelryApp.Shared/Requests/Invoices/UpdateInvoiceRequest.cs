using JewelryApp.Shared.Requests.InvoiceItems;

namespace JewelryApp.Shared.Requests.Invoices;

public record UpdateInvoiceRequest(int InvoiceId, string CustomerName, string CustomerPhoneNumber, DateTime InvoiceDate,
    double? Debt, DateTime? DebtDate, double? AdditionalPrices, double? Discount, List<UpdateInvoiceItemRequest> InvoiceItems);