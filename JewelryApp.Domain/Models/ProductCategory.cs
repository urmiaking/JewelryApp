namespace JewelryApp.Data.Models;

public class ProductCategory : SoftDeleteModelBase
{
    public string Name { get; set; } = default!;
}