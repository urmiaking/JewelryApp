namespace JewelryApp.Client.ViewModels;

public class ProductListVm
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public double Weight { get; set; }
    public double Wage { get; set; }
    public string WageType { get; set; } = default!;
    public string ProductType { get; set; } = default!;
    public string CaratType { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public string Barcode { get; set; } = default!;
    public bool Deleted { get; set; }
}