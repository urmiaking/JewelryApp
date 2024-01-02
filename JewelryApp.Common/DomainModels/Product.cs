using JewelryApp.Core.Enums;
using JewelryApp.Shared.Enums;

namespace JewelryApp.Core.DomainModels;

public class Product : SoftDeleteModelBase
{
    public string Name { get; set; } = default!;
    public string Barcode { get; set; } = default!;

    public double Weight { get; set; }
    public double Wage { get; set; }

    public CaratType Carat { get; set; }
    public WageType WageType { get; set; }
    public ProductType ProductType { get; set; }

    public DateTime? SellDateTime { get; set; }

    public int ProductCategoryId { get; set; }
    public ProductCategory ProductCategory { get; set; } = default!;
}