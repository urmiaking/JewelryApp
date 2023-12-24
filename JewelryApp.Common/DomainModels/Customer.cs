using System.ComponentModel.DataAnnotations.Schema;

namespace JewelryApp.Core.DomainModels;

public class Customer : SoftDeleteModelBase
{
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}