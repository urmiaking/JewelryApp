using JewelryApp.Data.Models;

namespace JewelryApp.Models.ViewModels;

public class ProductIndexVm
{
    public Product Product { get; set; } = new();
    //public SelectList ProductTypeSelectList { get; set; } = ProductType.Jewelry.ToSelectList();
    //public SelectList CaretSelectList { get; set; } = Caret.Eighteen.ToSelectList();
    public List<Product> Products { get; set; } = new();
}
