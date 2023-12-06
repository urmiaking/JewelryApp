namespace JewelryApp.Data.Models;

public class Customer : SoftDeleteModelBase
{
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}