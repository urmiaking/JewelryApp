namespace JewelryApp.Business.Repositories.Interfaces;

public interface IRepository<TModel> where TModel : class
{
    void Add(TModel model);
    Task<int> SaveChangesAsync();
    IQueryable<TModel> Get();
}