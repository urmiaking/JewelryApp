using JewelryApp.Common.Enums;

namespace JewelryApp.Models.Dtos.Product;

public class ProductTableItemDto
{
    public int Id { get; set; }
    public string BarcodeText { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public double Wage { get; set; }
    public ProductType ProductType { get; set; }
    public Carat Carat { get; set; }
}