using System.ComponentModel.DataAnnotations.Schema;

namespace JewelryApp.Core.DomainModels;

public class Customer : SoftDeleteModelBase
{
    [ForeignKey(nameof(Invoice))]
    public override int Id { get => base.Id; set => base.Id = value; }
    public string FullName { get; set; } = default!;
    public string? PhoneNumber { get; set; }

}