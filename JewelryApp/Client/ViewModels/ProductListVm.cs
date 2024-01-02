using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Client.ViewModels;

public class ProductListVm
{
    public int Id { get; set; }

    [Display(Name = "نام جنس")]
    public string Name { get; set; } = default!;

    [Display(Name = "وزن")]
    public double Weight { get; set; }

    [Display(Name = "اجرت")]
    public double Wage { get; set; }

    [Display(Name = "نوع اجرت")]
    public string WageType { get; set; } = default!;

    [Display(Name = "نوع کالا")]
    public string ProductType { get; set; } = default!;

    [Display(Name = "عیار")]
    public string CaratType { get; set; } = default!;

    [Display(Name = "دسته بندی")]
    public string CategoryName { get; set; } = default!;

    [Display(Name = "بارکد")]
    public string Barcode { get; set; } = default!;
    public bool Deleted { get; set; }
}