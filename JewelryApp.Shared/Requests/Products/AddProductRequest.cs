namespace JewelryApp.Shared.Requests.Products;

public record AddProductRequest(string Name, double Weight, double Wage, int WageType, int ProductType, int CaratType, int CategoryId, string Barcode);