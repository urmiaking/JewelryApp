using JewelryApp.Data.Models;
using JewelryApp.Data;

namespace JewelryApp.Business.Repositories.Implementations;

public class CustomerRepository : RepositoryBase<Customer>
{
    public CustomerRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}