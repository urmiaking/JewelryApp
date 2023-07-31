using JewelryApp.Data;
using JewelryApp.Data.Models;

namespace JewelryApp.Business.Repositories.Implementations;

public class InvoiceRepository : RepositoryBase<Invoice>
{
    public InvoiceRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}