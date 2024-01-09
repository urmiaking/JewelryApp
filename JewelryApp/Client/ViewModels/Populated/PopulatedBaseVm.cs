using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels.Populated;

public class PopulatedBaseVm : IEquatable<ProductCategoryVm>
{
    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا نام را وارد کنید")]
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

    public override int GetHashCode() => Name.GetHashCode();

    public override string ToString() => Name;
}