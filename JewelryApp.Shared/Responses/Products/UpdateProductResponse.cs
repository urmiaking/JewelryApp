namespace JewelryApp.Shared.Responses.Products;

public record UpdateProductResponse(int Id, string Name, double Weight, double Wage,
    string WageType, string ProductType, string CaratType,
    int CategoryId, string Barcode);
