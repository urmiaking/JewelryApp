namespace JewelryApp.Shared.Responses.Products;

public record AddProductResponse(int Id, string Name, double Weight, double Wage,
    string WageType, string ProductType, string CaratType,
    string CategoryName, string Barcode);