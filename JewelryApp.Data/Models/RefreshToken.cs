namespace JewelryApp.Data.Models;

public class RefreshToken : ModelBase<Guid>
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = default!;

    public Guid JwtId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool Used { get; set; }

    public bool Invalidated { get; set; }
}