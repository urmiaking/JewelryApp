namespace JewelryApp.Core.DomainModels;

public class Customer : SoftDeleteModelBase
{
    public string FullName { get; set; } = default!;
    public string? PhoneNumber { get; set; }

    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
}