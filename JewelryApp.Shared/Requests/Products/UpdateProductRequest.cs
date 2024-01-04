namespace JewelryApp.Shared.Requests.Products;

public record UpdateProductRequest(int Id, string Name, double Weight, double Wage, string WageType, string ProductType, string CaratType, int CategoryId, string Barcode);
