namespace JewelryApp.Shared.Responses.InvoiceItems;

public record GetInvoiceItemResponse(int ProductId, string ProductName, double ProductWeight, int CategoryId, double ProductWage, int ProductWageType,
    int CaratType, double TaxOffset, double Profit, double Quantity, double Tax, double RawPrice);
