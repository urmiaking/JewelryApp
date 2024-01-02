using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels;

public class AddProductVm
{
    [Display(Name = "نام جنس")]
    [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
    public string Name { get; } = default!;

    [Display(Name = "وزن")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Weight { get; }

    [Display(Name = "اجرت")]
    [Range(0, 100, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Wage { get; }

    [Display(Name = "نوع اجرت")]
    public int WageType { get; }

    [Display(Name = "نوع اجرت")]
    public int ProductType { get; }

    [Display(Name = "عیار")]
    public int CaratType { get; }

    [Display(Name = "دسته بندی")]
    public int CategoryId { get; }

    [Display(Name = "بارکد")]
    public string Barcode { get; } = default!;

    public List<ProductCategoryVm> ProductCategories { get; set; } = new List<ProductCategoryVm>();
}
