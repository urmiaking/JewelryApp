using JewelryApp.Models.Dtos.Authentication;

namespace JewelryApp.Business.AppServices;

public interface IRefreshTokenService
{
    Task<Guid> AddAsync(Guid userId, TimeSpan lifeTime, Guid jwtId);
    Task<RefreshTokenDto> FindAsync(Guid id);
    Task SetUsedAsync(Guid id);
    Task SetInvalidatedAsync(Guid id);
}