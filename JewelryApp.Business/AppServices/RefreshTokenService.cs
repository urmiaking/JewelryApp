using JewelryApp.Business.Interfaces;
using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRepository<RefreshToken> _repository;

    public RefreshTokenService(IRepository<RefreshToken> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> AddAsync(Guid userId, TimeSpan lifeTime, Guid jwtId)
    {
        var model = new RefreshToken
        {
            UserId = userId,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.Add(lifeTime),
            JwtId = jwtId
        };

        await _repository.AddAsync(model, CancellationToken.None);

        return model.Id;
    }

    public async Task<RefreshToken?> FindAsync(Guid id)
        => await _repository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task SetUsedAsync(Guid id)
    {
        var model = await _repository.Entities
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (model != null)
        {
            model.Used = true;

            await _repository.UpdateAsync(model, CancellationToken.None);
        }
    }

    public async Task SetInvalidatedAsync(Guid id)
    {
        var model = await _repository.Entities
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (model != null)
        {
            model.Invalidated = true;

            await _repository.UpdateAsync(model, CancellationToken.None);
        }
    }
}