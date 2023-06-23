using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;

namespace JewelryApp.Models.ViewModels;

public class AdminDashboardVm
{
    public AdminDashboardVm()
    {
    }

    public Product Product { get; set; } = new();
    //public SelectList ProductTypeSelectList { get; set; } = ProductType.Jewelry.ToSelectList();
    //public SelectList CaretSelectList { get; set; } = Caret.Eighteen.ToSelectList();
    public Invoice Invoice { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public List<Invoice> Invoices { get; set; } = new();
}