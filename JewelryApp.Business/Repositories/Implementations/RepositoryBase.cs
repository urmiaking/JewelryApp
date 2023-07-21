using JewelryApp.Business.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.Repositories.Implementations;

public class RepositoryBase<TModel> : IRepository<TModel> where TModel : class
{
    private readonly DbContext _dbContext;

    protected RepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual void Add(TModel model)
    {
        _dbContext.Add(model);
    }

    public virtual Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    public IQueryable<TModel> Get()
    {
        return _dbContext.Set<TModel>().AsQueryable();
    }
}