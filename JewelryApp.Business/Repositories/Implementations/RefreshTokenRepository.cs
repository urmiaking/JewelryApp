using JewelryApp.Data;
using JewelryApp.Data.Models;

namespace JewelryApp.Business.Repositories.Implementations;

public class RefreshTokenRepository : RepositoryBase<RefreshToken>
{
    public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}