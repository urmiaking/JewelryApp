using JewelryApp.Client.ViewModels.Populated;
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
    public int WageType { set; get; }

    [Display(Name = "نوع اجرت")]
    public int ProductType { set; get; }

    [Display(Name = "عیار")]
    public int CaratType { set; get; }

    [Display(Name = "دسته بندی")]
    public int CategoryId { set; get; }

    [Display(Name = "بارکد")]
    public string Barcode { set; get; } = default!;

    public ProductTypeVm ProductTypeVm { get; set; } = new();
    public CaratTypeVm CaratTypeVm { get; set; } = new();
    public WageTypeVm WageTypeVm { get; set; } = new();

    public List<ProductCategoryVm> ProductCategories { get; set; } = new ();
}
