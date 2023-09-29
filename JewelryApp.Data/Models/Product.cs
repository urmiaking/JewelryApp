using JewelryApp.Common.Enums;

namespace JewelryApp.Data.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Carat Carat { get; set; }
    public double Weight { get; set; }
    public double Wage { get; set; }
    public string Barcode { get; set; }
    public ProductType ProductType { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
}