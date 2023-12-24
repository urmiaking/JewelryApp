namespace JewelryApp.Shared.Responses.Products;

public record GetProductResponse(int Id, string Name, double Weight, double Wage,
    string WageType, string ProductType, string CaratType,
    string CategoryName, string Barcode, bool Deleted);
