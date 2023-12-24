namespace JewelryApp.Shared.Responses.Products;

public record AddProductResponse(int Id, string Name, double Weight, double Wage,
    int WageType, int ProductType, int CaratType,
    int CategoryName, string Barcode);