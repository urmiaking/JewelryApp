namespace JewelryApp.Models.Dtos;

public abstract record RefreshTokenDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid JwtId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool Used { get; set; }

    public bool Invalidated { get; set; }
}