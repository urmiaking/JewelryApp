using JewelryApp.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace JewelryApp.Models.Dtos.Product;

public class ProductDto
{
    public int Id { get; set; } = 0;

    [Display(Name = "نام جنس")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "عیار")]
    public Carat Carat { get; set; } = Carat.Eighteen;

    [Display(Name = "وزن")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public double Weight { get; set; }

    [Display(Name = "نوع")]
    public ProductType ProductType { get; set; } = ProductType.Gold;

    [Display(Name = "اجرت")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public double Wage { get; set; }

    public string BarcodeText { get; set; } = string.Empty;
}