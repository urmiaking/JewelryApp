using JewelryApp.Shared.Enums;

namespace JewelryApp.Client.ViewModels.Populated;

public class ProductTypeVm : PopulatedBaseVm
{
    public static IEnumerable<ProductTypeVm> GetProductTypes()
    {
        return from ProductType productType in Enum.GetValues(typeof(ProductType)) select new ProductTypeVm { Id = (int)productType, Name = productType.GetDisplayName() };
    }
}