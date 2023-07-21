using JewelryApp.Common.Enums;

namespace JewelryApp.Models.Dtos;

public class ProductTableItemDto
{
    public int Index { get; set; }
    public int Id { get; set; }
    public string BarcodeText { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ProductType ProductType { get; set; }
}