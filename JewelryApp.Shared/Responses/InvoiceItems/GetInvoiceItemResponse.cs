namespace JewelryApp.Shared.Responses.InvoiceItems;

public record GetInvoiceItemResponse(string Barcode, string Name, double Weight, string CategoryName, double Wage, string WageType,
    string CaratType, string ProductType, double TaxOffset, double Profit, double Tax, double Price, double GramPrice);
