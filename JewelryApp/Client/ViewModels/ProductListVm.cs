using System.ComponentModel.DataAnnotations;
using JewelryApp.Client.ViewModels.Populated;

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

    public static int GetCaratTypeId(string caratType)
    {
        return CaratTypeVm.GetCarats().FirstOrDefault(x => x.Name.Equals(caratType))!.Id;
    }

    public static int GetProductTypeId(string productType)
    {
        return ProductTypeVm.GetProductTypes().FirstOrDefault(x => x.Name.Equals(productType))!.Id;
    }

    public static int GetWageTypeId(string wageType)
    {
        return WageTypeVm.GetWageTypes().FirstOrDefault(x => x.Name.Equals(wageType))!.Id;
    }

    public CaratTypeVm CaratTypeVm { get; set; } = new CaratTypeVm();
    public ProductTypeVm ProductTypeVm { get; set; } = new ProductTypeVm();
    public WageTypeVm WageTypeVm { get; set; } = new WageTypeVm();
}