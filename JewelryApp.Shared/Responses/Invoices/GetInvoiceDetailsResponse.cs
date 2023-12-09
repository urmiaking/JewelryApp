using JewelryApp.Shared.Responses.InvoiceItems;
using JewelryApp.Shared.Responses.OldGolds;

namespace JewelryApp.Shared.Responses.Invoices;

public record GetInvoiceDetailsResponse(int Id, string CustomerName, string CustomerPhoneNumber, int CustomerId, DateTime InvoiceDate, 
    double Discount, double AdditionalPrices, double Difference, double Debt, 
    DateTime? DebtDate, double TotalRawPrice, double TotalTax, double TotalFinalPrice, 
    List<GetInvoiceItemResponse> InvoiceItems, List<GetOldGoldResponse> OldGolds);
