using JewelryApp.Data;
using JewelryApp.Data.Models;

namespace JewelryApp.Business.Repositories.Implementations;

public class InvoiceProductRepository : RepositoryBase<InvoiceItem>
{
    public InvoiceProductRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}