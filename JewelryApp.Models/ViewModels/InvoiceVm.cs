using System.ComponentModel.DataAnnotations;
using JewelryApp.Common.DateFunctions;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;

namespace JewelryApp.Models.ViewModels;

public class InvoiceVm
{
    public bool IsEdit { get; set; }
    public Invoice Invoice { get; set; } = new ();

    [Display(Name = "تاریخ فاکتور")]
    public string BuyDate { get; set; } = DateTime.Now.ToShamsiDateString();
    public List<ProductDto> Products { get; set; } = new();
    //public SelectList ProductTypeSelectList { get; set; } = ProductType.Jewelry.ToSelectList();
    //public SelectList CaretSelectList { get; set; } = Caret.Eighteen.ToSelectList();
    [Display(Name="نرخ روز گرم")]
    public double GramPrice { get; set; }
}
