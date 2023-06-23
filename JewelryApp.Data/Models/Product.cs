using System.ComponentModel.DataAnnotations;
using JewelryApp.Common.Enums;

namespace JewelryApp.Data.Models;

public class Product
{
    public int Id { get; set; }

    [Display(Name = "شرح جنس")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "عیار")]
    public Caret Caret { get; set; }

    [Display(Name = "وزن")]
    public double Weight { get; set; }
    public string BarcodeText { get; set; }

    [Display(Name = "نوع")]
    public ProductType ProductType { get; set; }
    public List<InvoiceProduct> InvoiceProducts { get; set; } = new ();
}