namespace JewelryApp.Shared.Responses.Products;

public record UpdateProductResponse(int Id, string Name, double Weight, double Wage,
    int WageType, int ProductType, int CaratType,
    int CategoryId, string Barcode);
