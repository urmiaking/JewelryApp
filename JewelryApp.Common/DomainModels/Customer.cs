namespace JewelryApp.Core.DomainModels;

public class Customer : SoftDeleteModelBase
{
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? NationalCode { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}