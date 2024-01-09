using JewelryApp.Client.ViewModels.Populated;
using JewelryApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels;

public class AddProductVm
{
    [Display(Name = "نام جنس")]
    [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
    public string Name { set; get; } = default!;

    [Display(Name = "وزن")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Weight { set; get; }

    [Display(Name = "اجرت")]
    [Range(0, 100, ErrorMessage = "وارد کردن {0} الزامی است")]
    public double Wage { set; get; }

    [Display(Name = "نوع اجرت")]
    public WageType WageType { set; get; } = WageType.Percent;

    [Display(Name = "نوع جنس")]
    public ProductType ProductType { set; get; } = ProductType.Gold;

    [Display(Name = "عیار")]
    public CaratType CaratType { set; get; } = CaratType.Eighteen;

    [Display(Name = "دسته بندی")]
    public ProductCategoryVm ProductCategory { set; get; } = new ProductCategoryVm() { Id = 0, Name = "انتخاب کنید" };

    [Display(Name = "بارکد")]
    public string Barcode { set; get; } = default!;

    public List<ProductCategoryVm> ProductCategories { get; set; } = new ();
}
