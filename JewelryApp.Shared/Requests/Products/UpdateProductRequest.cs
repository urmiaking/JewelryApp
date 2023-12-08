namespace JewelryApp.Shared.Requests.Products;

public record UpdateProductRequest(int Id, string Name, double Weight, double Wage, int WageType, int ProductType, int CaratType, int CategoryId, string Barcode);
