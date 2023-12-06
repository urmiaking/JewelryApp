using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.CommonDtos;

namespace JewelryApp.Models.Dtos.AuthenticationDtos;

public class RefreshTokenDto : BaseDto<RefreshTokenDto, RefreshToken, Guid>
{
    public Guid UserId { get; set; }

    public Guid JwtId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool Used { get; set; }

    public bool Invalidated { get; set; }
}