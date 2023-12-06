using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.CommonDtos;

namespace JewelryApp.Models.Dtos.ProductDtos;

public class ProductTableItemDto : BaseDto<ProductTableItemDto, Product>
{
    public int Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public double Wage { get; set; }
    public ProductType ProductType { get; set; }
    public Carat Carat { get; set; }
}