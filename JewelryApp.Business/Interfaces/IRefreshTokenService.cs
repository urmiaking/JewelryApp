using JewelryApp.Core.DomainModels;

namespace JewelryApp.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<Guid> AddAsync(Guid userId, TimeSpan lifeTime, Guid jwtId);
    Task<RefreshToken?> FindAsync(Guid id);
    Task SetUsedAsync(Guid id);
    Task SetInvalidatedAsync(Guid id);
}