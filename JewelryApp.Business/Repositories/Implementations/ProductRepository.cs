using JewelryApp.Data;
using JewelryApp.Data.Models;

namespace JewelryApp.Business.Repositories.Implementations;

public class ProductRepository : RepositoryBase<Product>
{
    public ProductRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}