namespace JewelryApp.Core.DomainModels;

public class ProductCategory : SoftDeleteModelBase
{
    public string Name { get; set; } = default!;
}