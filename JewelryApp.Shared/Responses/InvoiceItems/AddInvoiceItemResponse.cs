namespace JewelryApp.Shared.Responses.InvoiceItems;

public record AddInvoiceItemResponse(int Id, int ProductId, int InvoiceId, string ProductName, double ProductWeight, int CategoryId, double ProductWage, int ProductWageType,
    int CaratType, double TaxOffset, double Profit, double Quantity, double Tax, double RawPrice);