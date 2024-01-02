using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels;

public class ProductCategoryVm : IEquatable<ProductCategoryVm>
{
    public int Id { get; set; }

    [Display(Name = "نام دسته بندی")]
    public string Name { get; set; } = default!;

    public bool Equals(ProductCategoryVm? other)
    {
        if (other == null)
            return false;

        return other.Name == Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ProductCategoryVm other)
            return Equals(other);

        return false;
    }

    public override int GetHashCode() => Name?.GetHashCode() ?? 0;

    public override string ToString() => Name;
}
